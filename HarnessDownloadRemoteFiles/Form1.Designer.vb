<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.nudStartAt = New System.Windows.Forms.NumericUpDown()
        CType(Me.nudStartAt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnRun
        '
        Me.btnRun.Location = New System.Drawing.Point(78, 48)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(122, 63)
        Me.btnRun.TabIndex = 0
        Me.btnRun.Text = "Run Download"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(68, 144)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(151, 49)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Insert Missing Numbers"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'nudStartAt
        '
        Me.nudStartAt.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudStartAt.Location = New System.Drawing.Point(80, 212)
        Me.nudStartAt.Maximum = New Decimal(New Integer() {9999999, 0, 0, 0})
        Me.nudStartAt.Name = "nudStartAt"
        Me.nudStartAt.Size = New System.Drawing.Size(120, 20)
        Me.nudStartAt.TabIndex = 2
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.nudStartAt)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnRun)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Harness for Download Spocosy Files Service"
        CType(Me.nudStartAt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnRun As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents nudStartAt As NumericUpDown
End Class
