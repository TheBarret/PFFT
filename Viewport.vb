Public Class Viewport
    Inherits Panel
    Sub New()
        Me.DoubleBuffered = True
        Me.BackgroundImageLayout = ImageLayout.None
        Me.Font = New Font("Consolas", 8)
    End Sub

    Public Sub Render(data As Complex(), scaleFactor As Single)
        Using bm As New Bitmap(Me.ClientRectangle.Width - 5, Me.ClientRectangle.Height - 4)
            Using g As Graphics = Graphics.FromImage(bm)
                g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                g.Clear(Color.FromKnownColor(KnownColor.Control))

                ' Consider only the important part of the NQuist data
                Dim halfDataLength As Integer = data.Count \ 2
                ' Compute x-axis scale factor
                Dim xScaleFactor As Single = CSng(bm.Width) / data.Length

                For i As Integer = 0 To halfDataLength - 1
                    Dim magnitude As Double = data(i).Magnitude() * scaleFactor
                    Dim offset As Single = Math.Min(bm.Height, CSng(bm.Height * magnitude))
                    ' Multiply the x position by the scale factor
                    Dim x As Single = CSng((i / halfDataLength) * bm.Width)
                    Dim y As Single = CSng(bm.Height - offset)
                    g.FillRectangle(Brushes.CornflowerBlue, x, y, xScaleFactor, offset - 20)
                    ' Draw frequency markers and labels every 10th frequency bin
                    If i Mod 31 = 0 Then
                        g.DrawLine(Pens.Black, x, bm.Height, x, bm.Height - 10) ' Draw a small line marking the frequency
                        Dim label As String = String.Format("{0:F2}hz", (i / data.Length))
                        g.DrawString(label, Me.Font, Brushes.Black, x, bm.Height - 20) ' Draw the frequency label
                    End If
                Next
                g.DrawLine(Pens.Red, bm.Width \ 2, 0, bm.Width \ 2, bm.Height)
                g.DrawRectangle(Pens.Black, 0, 0, bm.Width - 1, bm.Height - 1)
            End Using
            Me.BackgroundImage = CType(bm.Clone, Image)
        End Using
    End Sub


End Class
