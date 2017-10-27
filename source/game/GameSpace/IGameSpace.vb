'the snake game
'(c) 2013 Pavel Jurča

Public Interface IGameSpace
    Sub add(ByVal ParamArray elements() As System.Windows.Forms.Control)
    Sub remove(ByVal ParamArray elements() As System.Windows.Forms.Control)
    Sub clear()
    Function getGridSize() As Integer
    Function getFreeGridP() As System.Drawing.Point()
    Function getCenterP() As System.Drawing.Point
End Interface