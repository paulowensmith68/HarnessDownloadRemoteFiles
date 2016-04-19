Imports System.Xml
Imports System.IO
Imports System.Text
Imports MySql.Data
Imports MySql.Data.MySqlClient

Public Class ScoposyFile

    ' Holds the connection string to the database used.
    Public connectionString As String = globalConnectionString

    ' Vars used for output message
    Private insertCount As Integer = 0
    Private updateCount As Integer = 0

    ' Vars used to control cursor
    Public intCursorCount As Integer = 0

    ' List of filenames to process
    Dim fileNameList As New List(Of String)

    ' Ftp variables
    Public ftp As New FTP(My.Settings.RemoteServerUser, My.Settings.RemoteServerPassword)

    ' Stream integer
    Public intNextStream As Integer = 0

    Public Sub DownloadFiles()
        '-----------------------------------------------------------------------*
        ' Sub Routine parameters                                                *
        '-----------------------------------------------------------------------*
        Dim cno As MySqlConnection = New MySqlConnection(connectionString)
        Dim dr As MySqlDataReader
        Dim cmd As New MySqlCommand

        ' Reset cursor counter
        intCursorCount = 0

        ' /----------------------------------------------------------------\
        ' | MySql Select                                                   |
        ' | Get all rows for nodeName from bookmaker_xml_nodes             |
        ' \----------------------------------------------------------------/
        cmd.CommandText = "Select id from saved_xml order by id"
        cmd.Connection = cno

        Try
            cno.Open()
            dr = cmd.ExecuteReader

            If dr.HasRows Then

                While dr.Read()

                    ' Increment counter
                    intCursorCount = intCursorCount + 1

                    Dim id = dr.GetInt32(0)
                    Dim fullFilename As New String(id.ToString + ".xml")
                    fileNameList.Add(fullFilename)


                    ' Leave cursor when we hit limit
                    If intCursorCount >= My.Settings.MaxFilesToDownload Then
                        Exit While
                    End If

                End While ' End: Outer Loop

            End If

            ' Close the Data reader
            dr.Close()

        Finally
            cno.Close()
        End Try


        ' Loop through the files, download, delete from saved_xml
        For Each filename In fileNameList

            ' Log activity
            gobjEvent.WriteToEventLog("ScoposyFile Class : Download remote files processing file: " + filename, EventLogEntryType.Information)

            ' Ftp download the file and remove from remote server
            FtpFile(filename)

            ' Delete the entry from saved_xml
            DeleteSavedXml(filename)

            ' Insert entry of saved_streammed_xml
            InsertLocalXml(filename)

            ' Log activity
            gobjEvent.WriteToEventLog("ScoposyFile Class : Download remote file processing completed", EventLogEntryType.Information)

        Next

        ftp = Nothing

    End Sub
    Public Sub FtpFile(filename As String)

        ' Set filenames
        Dim RFN As String = My.Settings.RemoteFtpServer + My.Settings.RemoteFtpPath + filename
        Dim LFN As String = My.Settings.LocalDownloadPath + filename

        Try

            ' Download file then delete
            ftp.NewDownload(LFN, RFN)

            ' Delete file
            ftp.DeleteFile(RFN)


        Catch ex As Exception

        End Try

    End Sub

    Private Sub DeleteSavedXml(filename As String)
        'Delete id from saved_xml (log table)
        Dim myConnection As New MySqlConnection(connectionString)
        Dim myCommand As New MySqlCommand("delete from saved_xml where id=@id")
        myCommand.CommandType = CommandType.Text
        myCommand.Connection = myConnection
        myCommand.Parameters.Add(New MySqlParameter("id", filename))

        Try

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            gobjEvent.WriteToEventLog("DownloadRemoteFiles : Delete of row saved_xml failed: " + ex.Message, EventLogEntryType.Error)

        Finally

            myConnection.Close()

        End Try

    End Sub

    Private Sub InsertLocalXml(filename As String)

        'Insert id into local_xml (log table)
        Dim myConnection As New MySqlConnection(connectionString)
        Dim myCommand As New MySqlCommand("INSERT INTO `oddsmatching`.`saved_streammed_xml` (`id`, `stream`) VALUES (@id, @stream)")
        myCommand.CommandType = CommandType.Text
        myCommand.Connection = myConnection

        ' Set stream number
        If intNextStream = 0 Then
            intNextStream = 1
        Else
            intNextStream = intNextStream + 1
            If intNextStream > My.Settings.NumberOfStreams Then
                intNextStream = 1
            End If
        End If

        Dim strId As String = filename.Replace(".xml", "")
        Dim id As Integer = Convert.ToInt32(strId)
        myCommand.Parameters.Add(New MySqlParameter("id", id))
        myCommand.Parameters.Add(New MySqlParameter("stream", intNextStream))

        Try

            myConnection.Open()
            myCommand.ExecuteNonQuery()

            ' Log activity
            gobjEvent.WriteToEventLog("ScoposyFile Class : Insert Local Xml successful for file: " + filename, EventLogEntryType.Information)

        Catch ex As Exception

            ' Log activity
            gobjEvent.WriteToEventLog("ScoposyFile Class : Insert Local Xml failed for file: " + filename + " Msg: " + ex.Message, EventLogEntryType.Error)

        Finally

            myCommand.Dispose()
            myConnection.Close()

        End Try

    End Sub

End Class
