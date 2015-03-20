'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

Public Class Wall
    Inherits System.Windows.Forms.Control

    Sub New(ByVal location As Point, ByVal size As Size, ByVal level As IGame.Level)
        Me.SetBounds(location.X, location.Y, size.Width, size.Height)
        Try
            Select Case level
                Case IGame.Level.LEVEL1
                    'Dim b As System.Drawing.Bitmap = New Bitmap(My.Resources.wallImg2, 40, 40)
                    Me.BackgroundImage = My.Resources.wallImg2
                Case IGame.Level.LEVEL2
                    Me.BackgroundImage = My.Resources.wallImg
            End Select
            Me.BackgroundImageLayout = ImageLayout.Tile
        Catch ex As Exception
            Me.BackColor = Color.Black
        End Try
    End Sub

    Public Function isClash(ByVal point As Point, ByVal size As Size) As Boolean
        If (point.X + size.Width > Me.Location.X AndAlso
            point.Y + size.Height > Me.Location.Y AndAlso
            point.X < Me.Location.X + Me.Width AndAlso
            point.Y < Me.Location.Y + Me.Height) Then
            Return True
        End If

        Return False
    End Function
End Class
