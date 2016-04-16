Public Class Form1
    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click

        Dim files As New ScoposyFile
        files.DownloadFiles()

    End Sub
End Class
