'the snake game
'(c) 2013 Pavel Jurča

Public Interface ISnake
    Enum Direction
        RIGHT = 0
        DOWN = 1
        LEFT = 2
        UP = 3
    End Enum
    Sub turn(ByVal dir As Direction)
    Sub move()
    Function grow(ByVal linksCount As Integer) As System.Windows.Forms.Control()
    Function shrink(ByVal linksCount As Integer) As System.Windows.Forms.Control()
    Function getHead() As System.Windows.Forms.Control
    Function getPoints() As System.Drawing.Point()
    Function getLength() As Integer
End Interface