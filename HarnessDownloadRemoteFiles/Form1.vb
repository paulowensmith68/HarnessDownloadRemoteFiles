Public Class Form1
    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click

        Dim files As New ScoposyFile
        files.DownloadFiles()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim file As ScoposyFile = New ScoposyFile()
        Dim intFile As Integer = nudStartAt.Value

        For i = 1 To 2029
            intFile = intFile + 10
            file.InsertRowSavedXml(intFile)
        Next

    End Sub

End Class
