'the snake game
'(c) 2013 Pavel Jurča

Option Strict On
Imports System.Drawing
Imports Microsoft.VisualBasic.VBMath

''' <summary>
''' Various methods for getting random results
''' </summary>
''' <remarks>Rand class has only class methods</remarks>
Public NotInheritable Class Rand

    Private Shared ReadOnly rand As Random = New Random()

    Private Sub New()
    End Sub

    Public Shared Function choiceSnakeDir() As ISnake.Direction
        'gets only right/down direction
        Select Case (rand.Next(0, 3))
            Case 0
                Return ISnake.Direction.RIGHT
            Case 1
                Return ISnake.Direction.RIGHT
            Case 2
                Return ISnake.Direction.DOWN
            Case Else
                Return ISnake.Direction.DOWN
        End Select
    End Function

    Public Shared Function choiceColor() As System.Drawing.Color
        'VBMath.Randomize(), VBMath.Rnd()
        Return Color.FromArgb(rand.Next(100, 225), rand.Next(25, 175), rand.Next(75, 200))
    End Function

    Public Overloads Shared Function choicePoint(ByVal points As Point()) As Point
        If (points.GetLength(0) = 0) Then Return New Point(0, 0)

        Return points(rand.Next(0, points.Length - 1))
    End Function

    'exclusive lower bound
    Public Overloads Shared Function choicePoint(ByVal points As Point(), ByVal lowBound As Integer) As Point
        If (points.GetLength(0) = 0) Then Return New Point(0, 0)

        Dim point As Point
        Do
            point = points(rand.Next(0, points.Length - 1))
        Loop Until Not (point.X > lowBound AndAlso point.Y > lowBound)

        Return point
    End Function

    'exclusive lower and upper bounds
    Public Overloads Shared Function choicePoint(ByVal points As Point(),
                                              ByVal lowBound As Integer,
                                              ByVal upBound As Integer) As Point

        If (points.GetLength(0) = 0) Then Return New Point(0, 0)

        Dim point As Point
        Do
            point = points(rand.Next(0, points.Length - 1))
        Loop While (point.X < lowBound Or point.X > upBound OrElse
                        point.Y < lowBound Or point.Y > upBound)

        Return point
    End Function
End Class
