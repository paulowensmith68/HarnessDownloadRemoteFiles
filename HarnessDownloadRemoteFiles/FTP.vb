Imports System.IO

Public Class FTP
    '----------------------------FTP-----------------------------
    Private _credentials As System.Net.NetworkCredential
    Sub New(ByVal _FTPUser As String, ByVal _FTPPass As String)
        setCredentials(_FTPUser, _FTPPass)
    End Sub
    Public Sub UploadFile(ByVal _FileName As String, ByVal _UploadPath As String)
        Dim _FileInfo As New System.IO.FileInfo(_FileName)
        Dim _FtpWebRequest As System.Net.FtpWebRequest = CType(System.Net.FtpWebRequest.Create(New Uri(_UploadPath)), System.Net.FtpWebRequest)
        _FtpWebRequest.Credentials = _credentials
        _FtpWebRequest.KeepAlive = False
        _FtpWebRequest.Timeout = 20000
        _FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile
        _FtpWebRequest.UseBinary = True
        _FtpWebRequest.ContentLength = _FileInfo.Length
        Dim buffLength As Integer = 2048
        Dim buff(buffLength - 1) As Byte
        Dim _FileStream As System.IO.FileStream = _FileInfo.OpenRead()
        Try
            Dim _Stream As System.IO.Stream = _FtpWebRequest.GetRequestStream()
            Dim contentLen As Integer = _FileStream.Read(buff, 0, buffLength)
            Do While contentLen <> 0
                _Stream.Write(buff, 0, contentLen)
                contentLen = _FileStream.Read(buff, 0, buffLength)
            Loop
            _Stream.Close()
            _Stream.Dispose()
            _FileStream.Close()
            _FileStream.Dispose()
        Catch ex As Exception
        End Try
    End Sub
    Public Sub DownloadFile(ByVal _FileName As String, ByVal _ftpDownloadPath As String)
        Try
            Dim _request As System.Net.FtpWebRequest = System.Net.WebRequest.Create(_ftpDownloadPath)
            _request.KeepAlive = False
            _request.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            _request.Credentials = _credentials
            Dim _response As System.Net.FtpWebResponse = _request.GetResponse()
            Dim responseStream As System.IO.Stream = _response.GetResponseStream()
            Dim fs As New System.IO.FileStream(_FileName, System.IO.FileMode.Create)
            responseStream.CopyTo(fs)
            responseStream.Close()
            _response.Close()

            gobjEvent.WriteToEventLog("FTP Class : FTP Download file completed successfully for file: " + _FileName, EventLogEntryType.Information)

        Catch ex As Exception

            gobjEvent.WriteToEventLog("FTP Class : FTP Download file failed for file: " + _FileName + " Msg: " + ex.Message, EventLogEntryType.Error)

        End Try
    End Sub
    Public Function GetDirectory(ByVal _ftpPath As String) As List(Of String)
        Dim ret As New List(Of String)
        Try
            Dim _request As System.Net.FtpWebRequest = System.Net.WebRequest.Create(_ftpPath)
            _request.KeepAlive = False
            _request.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails
            _request.Credentials = _credentials
            Dim _response As System.Net.FtpWebResponse = _request.GetResponse()
            Dim responseStream As System.IO.Stream = _response.GetResponseStream()
            Dim _reader As System.IO.StreamReader = New System.IO.StreamReader(responseStream)
            Dim FileData As String = _reader.ReadToEnd
            Dim Lines() As String = FileData.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            For Each l As String In Lines
                Dim words() As String = l.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                ret.Add(words(words.Count - 1))
            Next
            _reader.Close()
            _response.Close()

            gobjEvent.WriteToEventLog("FTP Class : FTP Get Directory completed successfully: " + _ftpPath, EventLogEntryType.Information)

        Catch ex As Exception

            gobjEvent.WriteToEventLog("FTP Class : FTP Get Directory failed for path: " + _ftpPath + " Msg: " + ex.Message, EventLogEntryType.Error)

        End Try
        Return ret
    End Function
    Public Function DeleteFile(ByVal _ftpPath As String) As String
        Dim ret As String = ""
        Try
            Dim _request As System.Net.FtpWebRequest = System.Net.WebRequest.Create(_ftpPath)
            _request.KeepAlive = False
            _request.Method = System.Net.WebRequestMethods.Ftp.DeleteFile
            _request.Credentials = _credentials
            Dim _response As System.Net.FtpWebResponse = _request.GetResponse()
            Dim responseStream As System.IO.Stream = _response.GetResponseStream()
            Dim _reader As System.IO.StreamReader = New System.IO.StreamReader(responseStream)
            ret = _reader.ReadToEnd
            _reader.Close()
            _response.Close()

            gobjEvent.WriteToEventLog("FTP Class : FTP Delete File completed successfully: " + _ftpPath, EventLogEntryType.Information)

        Catch ex As Exception

            gobjEvent.WriteToEventLog("FTP Class : FTP Delete File failed for path: " + _ftpPath + " Msg: " + ex.Message, EventLogEntryType.Error)

        End Try
        Return ret
    End Function

    Private Sub setCredentials(ByVal _FTPUser As String, ByVal _FTPPass As String)
        _credentials = New System.Net.NetworkCredential(_FTPUser, _FTPPass)
    End Sub

    Public Function NewDownload(ByVal _FileName As String, ByVal _ftpDownloadPath As String, Optional ByVal PermitOverwrite As Boolean = True) As Boolean

        Dim ftp As System.Net.FtpWebRequest = System.Net.WebRequest.Create(_ftpDownloadPath)
        ftp.KeepAlive = False
        ftp.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
        ftp.Credentials = _credentials
        'ftp.UseBinary = True

        Dim targetFI As New FileInfo(_FileName)

        'open request and get response stream
        Using response As Net.FtpWebResponse = CType(ftp.GetResponse, Net.FtpWebResponse)
            Using responseStream As Stream = response.GetResponseStream
                'loop to read & write to file
                Using fs As FileStream = targetFI.OpenWrite
                    Try
                        Dim buffer(2047) As Byte
                        Dim read As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                        Loop Until read = 0
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    Catch ex As Exception
                        'catch error and delete file only partially downloaded
                        fs.Close()
                        'delete target file as it's incomplete
                        targetFI.Delete()
                        'Throw
                        gobjEvent.WriteToEventLog("FTP Class : FTP failed for file: " + _FileName, EventLogEntryType.Error)
                    End Try
                End Using
                responseStream.Close()
            End Using
            response.Close()
        End Using
        Return True
    End Function
End Class