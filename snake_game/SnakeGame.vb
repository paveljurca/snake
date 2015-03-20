'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

''' <summary>
''' the snake game
''' </summary>
''' <remarks>"HasA relationship" of FormDesign, GameSpace, Snake, Food and Wall classes</remarks>
Public NotInheritable Class SnakeGame
    Implements IGame

    Private Const SPEED As Integer = 95 '10.5 grid per 1s
    Private Const TIMELIMITb As Integer = 2 'time limit for a bonus food in seconds
    Private level As IGame.Level
    Private name As String
    Private formDesign As FormDesign
    Private gameSpace As GameSpace
    Private snake As Snake
    Private food As Food
    Private bonusFood As Food
    Private walls As List(Of Wall)
    Private bonusTiming As Stopwatch 'stop watching of a bonus food
    Private WithEvents mover As Timer
    Private score As Long
    Private bingoSteps As List(Of Integer)
    Private bingoCount As Integer
    Private isRunning As Boolean 'new game has started

    Public Sub New(ByRef formDesign As FormDesign)
        name = "//" & My.Application.Info.Title
        Me.formDesign = formDesign
        checkFormDim(formDesign.getSize())
        gameSpace = GameSpace.getProstor(formDesign)
        formDesign.add(gameSpace)
        mover = New Timer()
        AddHandler mover.Tick, AddressOf mover_tick
    End Sub

    Public Sub newGame(ByVal level As IGame.Level) Implements IGame.newGame
        Me.level = level
        mover.Stop()
        mover.Interval = SPEED

        gameSpace.clear()

        walls = New List(Of Wall)
        bingoSteps = New List(Of Integer)
        bonusTiming = New Stopwatch
        score = 0 : bingoCount = 0
        isRunning = True

        'create and display a new snake and food
        snake = New Snake(New Point(0, 0),
                         gameSpace.getGridSize(),
                         Rand.choiceSnakeDir())
        gameSpace.add(snake.grow())

        handleLeveling() 'eventually build walls

        food = New Food(Rand.choicePoint(gameSpace.getFreeGridP()),
                gameSpace.getGridSize(),
                Rand.choiceColor())
        bonusFood = New Food(New Point(-100, -100),
                gameSpace.getGridSize(), level)
        gameSpace.add(food)

        formDesign.setCaption(String.Concat(name, " ", score, " p"))
        formDesign.focus()

        mover.Start() 'actually starts a new game of the snake game
    End Sub
    'pohyb hry
    Private Sub mover_tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not (isGameOver()) Then

            snake.move()
            For i As Integer = 0 To bingoSteps.Count - 1
                bingoSteps.Item(i) -= 1
            Next i

            handleLeveling()
            handleFood()
            If bingoSteps.Count > 0 AndAlso bingoSteps.Item(0) = 0 Then
                'food cannot be added until the snake moves again
                gameSpace.add(snake.grow())
                If (mover.Interval > 4) Then mover.Interval -= 1
                bingoSteps.RemoveAt(0)
            End If
        Else
            gameSpace.BackColor = Color.FromArgb(255, 71, 71) 'awareness color
            quit()
        End If
    End Sub

    Private Sub handleFood()
        Dim usualFBingo As Boolean = snake.getHead().Location.Equals(food.Location)
        Dim bonusFBingo As Boolean = snake.getHead().Location.Equals(bonusFood.Location)

        Select Case True 'bingo, the snake has already eaten his food
            Case usualFBingo
                bingoCount += 1
                score += 20
                snake.getHead.BackColor = food.BackColor

                gameSpace.remove(food)
                food = New Food(Rand.choicePoint(gameSpace.getFreeGridP()),
                    gameSpace.getGridSize(),
                    Rand.choiceColor())
                gameSpace.add(food)
                'every fourth usual food cause showing of a bonus food
                If (bingoCount Mod 4 = 0 AndAlso Not bonusTiming.IsRunning) Then
                    bonusFood = New Food(Rand.choicePoint(gameSpace.getFreeGridP()),
                        gameSpace.getGridSize(), level)
                    gameSpace.add(bonusFood)

                    bonusTiming = Stopwatch.StartNew
                End If
                bingoSteps.Add(snake.getLength())
            Case bonusFBingo AndAlso bonusTiming.IsRunning
                'bonus food only gets you half of the actual score in addition
                'the snake won't grow
                score += CLng(score * 1 / 2)

                gameSpace.remove(bonusFood)
                bonusTiming.Reset()
        End Select
        '
        If (bonusTiming.Elapsed.TotalSeconds >= TIMELIMITb) Then
            gameSpace.remove(bonusFood)
            bonusTiming.Reset()
        End If
        '
        If (usualFBingo OrElse bonusFBingo) Then
            formDesign.setCaption(String.Concat(name, " ", score, " p"))
        End If
    End Sub

    Private Sub handleLeveling()
        Select Case level
            Case IGame.Level.LEVEL0
                'If (score > 1000) Then
                'mover.Stop()
                'MsgBox("> You're in LEVEL #1 now <" & vbNewLine & vbNewLine &
                '       Space(7) & "level#0 completed!", vbOKOnly, name)
                'newGame(IGame.Level.LEVEL1)
                'If walls.Count = 0 Then wallLevel1()
                'End If
            Case IGame.Level.LEVEL1
                'If (score > 1000) Then
                '    mover.Stop()
                '    MsgBox("> You're in LEVEL #2 now <" & vbNewLine & vbNewLine &
                '           Space(7) & "level#1 completed!", vbOKOnly, name)
                '    newGame(IGame.Level.LEVEL2)
                'End If
                If walls.Count = 0 Then wallLevel1()
            Case IGame.Level.LEVEL2
                If walls.Count = 0 Then wallLevel2()

                '**walking through boundaries
                'vertical boundaries
                Select Case (snake.getHead().Location.X)
                    Case Is < 0
                        snake.getHead().Location = Point.Add(
                            snake.getHead().Location, New Size(gameSpace.Width, 0))
                    Case Is >= gameSpace.Width
                        snake.getHead().Location = Point.Subtract(
                            snake.getHead().Location, New Size(gameSpace.Width, 0))
                End Select
                'horizontal boundaries
                Select Case (snake.getHead().Location.Y)
                    Case Is < 0
                        snake.getHead().Location = Point.Add(
                            snake.getHead().Location, New Size(0, gameSpace.Height))
                    Case Is >= gameSpace.Height
                        snake.getHead().Location = Point.Subtract(
                            snake.getHead().Location, New Size(0, gameSpace.Height))
                End Select
        End Select
    End Sub

    Private Sub wallLevel1()
        Dim squareSize As Integer = gameSpace.getGridSize()
        'horizontal top left
        walls.Add(New Wall(New Point(squareSize * 8, squareSize * 4),
                           New Size(squareSize * 4, squareSize * 2), level))
        gameSpace.add(walls.Last)
        'horizontal bottom left
        walls.Add(New Wall(New Point(squareSize * 8, gameSpace.Height - squareSize * 6),
                                 New Size(squareSize * 4, squareSize * 2), level))
        gameSpace.add(walls.Last)
        'horizontal top right
        walls.Add(New Wall(New Point(gameSpace.Width - squareSize * 12, squareSize * 4),
                           New Size(squareSize * 4, squareSize * 2), level))
        gameSpace.add(walls.Last)
        'horizontal bottom right
        walls.Add(New Wall(New Point(gameSpace.Width - squareSize * 12,
                                     gameSpace.Height - squareSize * 6),
                                 New Size(squareSize * 4, squareSize * 2), level))
        gameSpace.add(walls.Last)
        'vertical center
        walls.Add(New Wall(New Point(gameSpace.Width \ 2 - squareSize, squareSize * 8),
                          New Size(squareSize * 2, squareSize * 4), level))
        gameSpace.add(walls.Last)
    End Sub

    Private Sub wallLevel2()
        Dim squareSize As Integer = gameSpace.getGridSize()
        'horizontal bottom
        walls.Add(New Wall(New Point(0, gameSpace.Height - squareSize * 6),
                          New Size(gameSpace.Width, squareSize), level))
        gameSpace.add(walls.Last)
        'vertical center
        walls.Add(New Wall(New Point(gameSpace.Width \ 2, 0),
                          New Size(squareSize, gameSpace.Height - squareSize * 5), level))
        gameSpace.add(walls.Last)
        'horizontal top
        walls.Add(New Wall(New Point(gameSpace.Width \ 2 - squareSize * 3, squareSize * 5),
                          New Size(gameSpace.Width - (gameSpace.Width \ 2 + squareSize), squareSize), level))
        gameSpace.add(walls.Last)
    End Sub

    Public Function isGameOver() As Boolean Implements IGame.isGameOver
        If Not (snake Is Nothing) Then
            'in level#2 you can go through boundaries
            If (level <> IGame.Level.LEVEL2) Then
                'axis X
                Select Case (snake.getHead().Location.X)
                    Case Is < 0
                        Return True
                    Case Is >= gameSpace.Width
                        Return True
                End Select
                'axis Y
                Select Case (snake.getHead().Location.Y)
                    Case Is < 0
                        Return True
                    Case Is >= gameSpace.Height
                        Return True
                End Select
            End If
            'body-of-the-snake check
            Dim body() As Point = snake.getPoints()
            For i As Integer = 1 To UBound(body)
                If body(0).Equals(body(i)) Then
                    'snake has crashed into his own body
                    Return True
                End If
            Next i
            'walls as the case may be
            For Each w As Wall In walls
                If (w.isClash(snake.getHead().Location, snake.getHead().Size)) Then
                    Return True
                End If
            Next w
        End If


        Return False
    End Function

    Public Sub processKey(ByVal key As System.Windows.Forms.KeyEventArgs) Implements IGame.processKey
        If (isRunning) Then
            'you cannot change a snake's direction whilst the snake is not moving
            If (Not mover.Enabled AndAlso Not key.KeyCode.Equals(Keys.Space)) Then Exit Sub

            Select Case key.KeyCode
                Case Keys.Space : pause(mover.Enabled)
                Case Keys.Right : snake.turn(ISnake.Direction.RIGHT)
                Case Keys.Down : snake.turn(ISnake.Direction.DOWN)
                Case Keys.Left : snake.turn(ISnake.Direction.LEFT)
                Case Keys.Up : snake.turn(ISnake.Direction.UP)
            End Select
        End If
    End Sub

    'starts or stops the timer
    Private Sub pause(ByVal pause As Boolean)
        Select Case pause
            Case True : mover.Stop()
                formDesign.setCaption(String.Concat("//press SPACE to continue"))
            Case False : mover.Start()
                formDesign.setCaption(String.Concat(name, " ", score, " p"))
        End Select
    End Sub

    Public Sub quit() Implements IGame.quit
        isRunning = False
        mover.Stop()
        Application.Exit()
    End Sub

    Public Function getSpeed() As Integer Implements IGame.getSpeed
        'in pixels per one second
        Return CInt((1000 / mover.Interval) * gameSpace.getGridSize())
    End Function

    Public Function getScore() As Long Implements IGame.getScore
        Return score
    End Function

    Public Function getLevel() As Integer Implements IGame.getLevel
        Return level
    End Function

    Public Function getName() As String Implements IGame.getName
        Return name
    End Function

    Private Sub checkFormDim(ByVal size As Size) 'throws ArgumentException
        If Not (size.Width Mod 100 = 0 And size.Height Mod 100 = 0) Then
            Throw New ArgumentException("Dimensions of this form are not Mod 100")
        ElseIf Not ((size.Width / 100) / (size.Height / 100) = 3 / 2) Then
            Throw New ArgumentException("Dimensions of this form are not in 3 : 2 ratio")
        End If
    End Sub
End Class
