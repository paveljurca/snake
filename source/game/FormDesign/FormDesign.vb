'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

Imports System.Windows.Forms

Public Class FormDesign
    Implements IFormDesign

    Private form As Form
    Private size As Size
    Private caption As String
    Private borderStyle As FormBorderStyle

    Public Sub New(ByRef f As System.Windows.Forms.Form, ByVal size As Size, ByVal caption As String, ByVal borderStyle As FormBorderStyle)
        Me.form = f
        Me.size = size
        Me.caption = caption
        Me.borderStyle = BorderStyle

        f.Size = size
        f.Text = caption
        f.FormBorderStyle = borderStyle
    End Sub

    Public Overridable Sub centerOnDesktop() Implements IFormDesign.centerOnDesktop
        form.StartPosition = FormStartPosition.Manual
        form.SetDesktopLocation(
            (My.Computer.Screen.WorkingArea.Width \ 2) - (size.Width \ 2 + 85),
            (My.Computer.Screen.WorkingArea.Height \ 2) - (size.Height \ 2 + 85))
    End Sub

    Public Overridable Sub disableMinMax() Implements IFormDesign.disableMinMax
        form.MinimizeBox = False
        form.MaximizeBox = False
    End Sub

    Public Overridable Sub setSize(ByVal size As System.Drawing.Size) Implements IFormDesign.setSize
        Me.size = size
        form.Size = Me.size
    End Sub

    Public Overridable Function getSize() As Size Implements IFormDesign.getSize
        Return size
    End Function

    Public Overridable Sub disableResizing() Implements IFormDesign.disableResizing
        form.MinimumSize = size
        form.MaximumSize = size
    End Sub

    Public Overridable Sub allowResizing() Implements IFormDesign.allowResizing
        form.MinimumSize = New Size(0, 0)
        form.MaximumSize = New Size(0, 0)
    End Sub

    Public Overridable Sub focus() Implements IFormDesign.focus
        form.Focus()
    End Sub

    Public Overridable Function setCaption(ByVal caption As String) As String Implements IFormDesign.setCaption
        form.Text = caption
        Dim oldCaption As String = Me.caption
        Me.caption = caption

        Return oldCaption
    End Function

    Public Overridable Sub add(ByVal ParamArray elements() As Control) Implements IFormDesign.add
        form.Controls.AddRange(elements)
    End Sub

    Sub remove(ByVal ParamArray elements() As Control) Implements IFormDesign.remove
        For Each remove As Control In elements
            If (form.Controls.Contains(remove)) Then form.Controls.Remove(remove)
        Next remove
    End Sub

    Public Overridable Function contains(ByVal element As Control) As Boolean Implements IFormDesign.contains
        Return form.Controls.Contains(element)
    End Function

    Public Function getElements() As Control() Implements IFormDesign.getElements
        Dim controls(0 To form.Controls.Count - 1) As Control
        form.Controls.CopyTo(controls, 0)

        Return controls
    End Function

    Public Overridable Sub clear() Implements IFormDesign.clear
        form.Controls.Clear()
    End Sub
End Class