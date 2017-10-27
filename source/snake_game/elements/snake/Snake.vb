'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Snake entity
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class Snake
    Implements ISnake

    Private Const DEFAULTGROW As Integer = 1
    Private linkSize As Integer
    Private directions As List(Of Point)
    Private lastLoc As Point 'location of the preceding link in a snake's body
    Private color As Color
    Private links As List(Of Control)

    Public Sub New(ByVal startLoc As Point, ByVal linkSize As Integer, ByVal direction As ISnake.Direction)
        init(startLoc, linkSize, direction)
    End Sub

    Private Sub init(ByVal startLoc As Point, ByVal linkSize As Integer, ByVal direction As ISnake.Direction)
        links = New List(Of Control)
        directions = New List(Of Point)
        Me.linkSize = linkSize
        lastLoc = New Point(startLoc.X, startLoc.Y)
        'initial direction
        turn(direction)

        color = color.FromArgb(225, 225, 225)
    End Sub

    Public Sub move() Implements ISnake.move
        Dim _p As Point 'temp point
        Dim i As Integer = -1
        For Each link As Control In links
            i += 1
            If (i = 0) Then 'snake's head
                lastLoc = link.Location
                If directions.Count > 1 Then directions.RemoveAt(0)
                link.Location = New Point(link.Location.X + directions.First().X,
                                           link.Location.Y + directions.First().Y)
                Continue For
            End If
            _p = link.Location
            link.Location = lastLoc
            lastLoc = _p
        Next link
    End Sub

    Public Overloads Function grow(ByVal linksCount As Integer) As System.Windows.Forms.Control() Implements ISnake.grow
        If linksCount < 1 Then Return Nothing
        Dim _links(linksCount - 1) As Control
        Dim link As Control

        For i As Integer = 0 To linksCount - 1
            link = New Control()
            link.SetBounds(lastLoc.X, lastLoc.Y, linkSize, linkSize)
            link.BackColor = color

            links.Add(link) : _links(i) = link
        Next i

        Return _links
    End Function

    Public Overloads Function grow() As System.Windows.Forms.Control()
        Return grow(DEFAULTGROW)
    End Function

    Public Function shrink(ByVal linksCount As Integer) As System.Windows.Forms.Control() Implements ISnake.shrink
        If linksCount < 1 Or linksCount > links.Count Then Return New Control() {}
        lastLoc = links.Item(links.Count - linksCount).Location

        Dim remove() As Control = links.GetRange(links.Count - linksCount, linksCount).ToArray
        For Each r As Control In remove
            links.Remove(r)
        Next r

        Return remove
    End Function

    Function getHead() As System.Windows.Forms.Control Implements ISnake.getHead
        Return links.FirstOrDefault()
    End Function

    Public Function getPoints() As System.Drawing.Point() Implements ISnake.getPoints
        Dim points(links.Count - 1) As Point
        Dim i As Integer = 0
        For Each link As Control In links
            points(i) = link.Location
            i += 1
        Next link

        Return points
    End Function

    Public Function getLength() As Integer Implements ISnake.getLength
        Return links.Count
    End Function

    Public Sub turn(ByVal dir As ISnake.Direction) Implements ISnake.turn
        Select Case dir
            Case ISnake.Direction.RIGHT
                If Not directions.LastOrDefault().X < 0 Then _
                    directions.Add(New Point(linkSize, 0))
            Case ISnake.Direction.DOWN
                If Not directions.LastOrDefault().Y < 0 Then _
                    directions.Add(New Point(0, linkSize))
            Case ISnake.Direction.LEFT
                If Not directions.LastOrDefault().X > 0 Then _
                    directions.Add(New Point(linkSize * -1, 0))
            Case ISnake.Direction.UP
                If Not directions.LastOrDefault().Y > 0 Then _
                    directions.Add(New Point(0, linkSize * -1))
        End Select
    End Sub
End Class
