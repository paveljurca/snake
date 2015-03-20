'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

Public Class Food
    Inherits System.Windows.Forms.Control

    ''' <summary>
    ''' Usual food
    ''' </summary>
    Sub New(ByVal location As Point, ByVal size As Integer, ByVal color As Color)
        Me.SetBounds(location.X, location.Y, size, size)
        Me.BackColor = Color
    End Sub

    ''' <summary>
    ''' Bonus food
    ''' </summary>
    Sub New(ByVal location As Point, ByVal size As Integer, ByVal level As IGame.Level)
        Me.SetBounds(location.X, location.Y, size, size)
        Try
            Select Case level
                Case IGame.Level.LEVEL0
                    Me.BackgroundImage = My.Resources.foodImg
                Case IGame.Level.LEVEL1
                    Me.BackgroundImage = My.Resources.foodImg2
                Case IGame.Level.LEVEL2
                    Me.BackgroundImage = My.Resources.foodImg3
            End Select
            Me.BackgroundImageLayout = ImageLayout.Stretch
        Catch ex As Exception
            Me.BackColor = Color.Yellow
        End Try
    End Sub
End Class
