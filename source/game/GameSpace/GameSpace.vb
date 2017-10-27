'the snake game
'(c) 2013 Pavel Jurča

Option Strict On
Imports System.Windows.Forms
Imports System.Drawing

''' <summary>
''' The game space
''' </summary>
''' <remarks>implemented as a Panel</remarks>
Public Class GameSpace
    Inherits System.Windows.Forms.Panel
    Implements IGameSpace

    Public Const GRID As Double = 1 / 20 'square size
    Public Const _MARGIN As Integer = 1 'every grid of the GameSpace has so-called 1px "Margin.All"
    Private Shared gameSpace As GameSpace
    Private ReadOnly defaultColor As Color = Color.FromArgb(255, 250, 240) 'barva pozadi herniho prostoru

    Private Sub New(ByVal form As FormDesign)
        init(form)
    End Sub
    'singleton pattern
    Public Shared Function getProstor(ByVal form As FormDesign) As GameSpace
        If gameSpace Is Nothing Then gameSpace = New GameSpace(form)

        Return gameSpace
    End Function

    Public Overridable Sub add(ByVal ParamArray elements() As System.Windows.Forms.Control) Implements IGameSpace.add
        'workaround with margins
        For i As Integer = 0 To UBound(elements)
            elements(i).Location = Point.Add(elements(i).Location, New Size(_MARGIN, _MARGIN))
            elements(i).Size = Size.Subtract(elements(i).Size, New Size(_MARGIN * 2, _MARGIN * 2))
        Next i

        Me.Controls.AddRange(elements)
    End Sub

    Public Overridable Sub remove(ByVal ParamArray elements() As System.Windows.Forms.Control) Implements IGameSpace.remove
        For Each remove As Control In elements
            If (Me.Controls.Contains(remove)) Then Me.Controls.Remove(remove)
        Next remove
    End Sub

    Public Overridable Sub clear() Implements IGameSpace.clear
        Me.Controls.Clear()
        Me.BackColor = defaultColor
        Me.BringToFront()
    End Sub

    Public Overridable Function getGridSize() As Integer Implements IGameSpace.getGridSize
        Return findOutGridS()
    End Function

    Public Overridable Function getFreeGridP() As System.Drawing.Point() Implements IGameSpace.getFreeGridP
        Dim grid As Integer = findOutGridS()
        Dim gridPoints As New List(Of Point)

        For row As Integer = 0 To (Me.Height - grid) Step grid
            For column As Integer = 0 To (Me.Width - grid) Step grid
                gridPoints.Add(New Point(column, row))
                'check if there's a space for a grid in the gamespace's grid
                For Each c As Control In Me.Controls
                    If (Not c.Location.X >= column + grid AndAlso
                        Not c.Location.Y >= row + grid) Then
                        If (column - c.Location.X - c.Width < 0 AndAlso
                            row - c.Location.Y - c.Height < 0) Then
                            gridPoints.Remove(gridPoints.Last())
                            Exit For
                        End If
                    End If
                Next c
            Next column
        Next row

        Return gridPoints.ToArray
    End Function

    Public Overridable Function getCenterP() As System.Drawing.Point Implements IGameSpace.getCenterP
        Return New Point(CInt(Me.Width / 2 - ((Me.Width / 2) Mod findOutGridS())),
                         CInt(Me.Height / 2 - ((Me.Height / 2) Mod findOutGridS())))
    End Function

    Private Sub init(ByRef form As FormDesign)
        For Each element As Control In form.getElements
            element.Visible = False
        Next element

        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.Transparent

        form.add(Me) 'VERY IMPORTANT

        form.allowResizing()
        Dim w As Integer = form.getSize().Width
        Dim h As Integer = form.getSize().Height
        'width and height of a GameSpace class need to be set properly
        'GameSpace must have the same dimension as the form has
        Do
            Select Case (True)
                Case (Me.Width < w AndAlso Me.Height < h)
                    form.setSize(
                        New Size(form.getSize().Width + 1,
                                 form.getSize.Height + 1))
                Case (Me.Width < w)
                    form.setSize(
                        New Size(form.getSize().Width + 1,
                                 form.getSize().Height))
                Case (Me.Height < h)
                    form.setSize(
                        New Size(form.getSize().Width,
                                 form.getSize().Height + 1))
            End Select
        Loop Until (Me.Width = w And Me.Height = h) : form.disableResizing()

        form.remove(Me) 'VERY IMPORTANT
        Me.BackColor = defaultColor
    End Sub

    Private Function findOutGridS() As Integer
        'one (square) grid has a 1/20 of the size of GameSpace's width/height _
        '(depends on which dimension is the smaller one)
        Select Case (Me.Width >= Me.Height)
            Case True
                Return CInt(Me.Height * GRID)
            Case Else
                Return CInt(Me.Width * GRID)
        End Select
    End Function
End Class
