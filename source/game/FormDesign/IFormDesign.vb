'the snake game
'(c) 2013 Pavel Jurča

Imports System.Windows.Forms.Control

Public Interface IFormDesign
    Sub centerOnDesktop()
    Sub disableMinMax() 'disable minimalization and maximalization of a form
    Sub setSize(ByVal size As System.Drawing.Size)
    Function getSize() As Size
    Sub disableResizing()
    Sub allowResizing()
    Sub focus()
    Function setCaption(ByVal caption As String) As String 'gets the old version of a caption
    Sub add(ByVal ParamArray elements() As Control)
    Sub remove(ByVal ParamArray elements() As Control)
    Function contains(ByVal element As Control) As Boolean
    Function getElements() As Control()
    Sub clear()
End Interface