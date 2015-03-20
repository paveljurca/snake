'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

Public Interface IGame
    Enum Level
        LEVEL0 = 0
        LEVEL1 = 1
        LEVEL2 = 2
    End Enum
    Sub processKey(ByVal klavesa As System.Windows.Forms.KeyEventArgs)
    Sub newGame(ByVal level As IGame.Level)
    Sub quit()
    Function isGameOver() As Boolean
    Function getSpeed() As Integer
    Function getName() As String
    Function getScore() As Long
    Function getLevel() As Integer
End Interface