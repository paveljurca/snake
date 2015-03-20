'the snake game
'(c) 2013 Pavel Jurča

Option Strict On

Public Class Form1

    Private formDesign As FormDesign
    Private WithEvents btnNewGame As Button
    Private groupBox As GroupBox
    Private radBtnLevel0 As RadioButton
    Private radBtnLevel1 As RadioButton
    Private radBtnLevel2 As RadioButton
    Private snakeGame As SnakeGame
    Private gameLevel As IGame.Level = IGame.Level.LEVEL0

    '**** < EVENTS > ****
    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Select Case e.CloseReason
            Case CloseReason.ApplicationExitCall
                If Not (snakeGame Is Nothing) AndAlso snakeGame.isGameOver Then
                    Dim result As DialogResult
                    result = MessageBox.Show(String.Concat("> level #", snakeGame.getLevel(), vbNewLine, _
                                                          "> earned ", snakeGame.getScore(), " points", vbNewLine, _
                                                          "> speed of ", snakeGame.getSpeed() & " pixels /1s", _
                                                          vbNewLine, vbNewLine, _
                                                          "Wanna PLAY again?"), snakeGame.getName() & " is OVER !",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                      MessageBoxDefaultButton.Button1)
                    If result = Windows.Forms.DialogResult.Yes Then e.Cancel = True : snakeGame.newGame(gameLevel)
                    ''If result = Windows.Forms.DialogResult.Yes Then e.Cancel = True : formDesign.clear() : init()
                End If
        End Select
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        init()
    End Sub

    '** < HERE NEW GAME STARTS > **
    Private Sub btnNewGame_Clicked(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If (radBtnLevel0.Checked) Then gameLevel = IGame.Level.LEVEL0
            If (radBtnLevel1.Checked) Then gameLevel = IGame.Level.LEVEL1
            If (radBtnLevel2.Checked) Then gameLevel = IGame.Level.LEVEL2

            snakeGame = New SnakeGame(formDesign)
            snakeGame.newGame(gameLevel)
        Catch ex As ArgumentException
            MessageBox.Show(ex.Message & vbNewLine & vbNewLine & "> The game is going to close <",
                            "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation)
            End 'Me.Close()
        End Try
    End Sub
    '** < /HERE NEW GAME STARTS > **

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If Not snakeGame Is Nothing Then snakeGame.processKey(e)
    End Sub
    '**** < /EVENTS > ****

    Private Sub init()
        formDesign = New FormDesign(Me, New Size(900, 600), "//" & My.Application.Info.Title & " 2013 @paveljurca",
                                      Windows.Forms.FormBorderStyle.SizableToolWindow)
        formDesign.disableMinMax() : formDesign.disableResizing()
        formDesign.centerOnDesktop()

        createControls()
        btnNewGame.Focus()
    End Sub

    '** < DESIGNING STARTUP SCREEN > **
    Private Sub createControls()
        'button
        btnNewGame = New Button
        With btnNewGame
            .SetBounds(Me.Width \ 2 - ((Me.Width \ 2) \ 2) - 10,
                                 Me.Height \ 2 - ((Me.Width \ 5) \ 2) - 7,
                                 Me.Width \ 2, Me.Height \ 5)
            .Text = "Play"
            .BackColor = Color.FromArgb(72, 210, 134)
            .ForeColor = Color.WhiteSmoke
            .Font = New Font("Courier New", btnNewGame.Height \ 3, FontStyle.Bold)
            .FlatStyle = FlatStyle.Flat
            .FlatAppearance.BorderSize = 3
            .FlatAppearance.BorderColor = Color.White
            .TabIndex = 0
        End With
        AddHandler btnNewGame.Click, AddressOf btnNewGame_Clicked
        formDesign.add(btnNewGame)
        'group box
        groupBox = New GroupBox
        With groupBox
            .SetBounds(btnNewGame.Location.X,
                               btnNewGame.Location.Y + btnNewGame.Height,
                               btnNewGame.Width, btnNewGame.Height \ 2)
            .TabIndex = 1
        End With
        formDesign.add(groupBox)
        'radio button "level 1"
        radBtnLevel1 = New RadioButton
        With radBtnLevel1
            .Text = "level #1"
            .TextAlign = ContentAlignment.MiddleCenter
            .CheckAlign = ContentAlignment.TopCenter
            .Dock = DockStyle.Fill
            .TabIndex = 1
        End With
        groupBox.Controls.Add(radBtnLevel1)
        'radio button "level 0"
        radBtnLevel0 = New RadioButton
        With radBtnLevel0
            .Text = "level #0"
            .TextAlign = ContentAlignment.MiddleCenter
            .CheckAlign = ContentAlignment.TopCenter
            .Dock = DockStyle.Left
            .TabIndex = 0
            .Checked = True
        End With
        groupBox.Controls.Add(radBtnLevel0)
        'radio button "level 2"
        radBtnLevel2 = New RadioButton
        With radBtnLevel2
            .Text = "level #2"
            .TextAlign = ContentAlignment.MiddleCenter
            .CheckAlign = ContentAlignment.TopCenter
            .Dock = DockStyle.Right
            .TabIndex = 2
        End With
        groupBox.Controls.Add(radBtnLevel2)
    End Sub
    '** < /DESIGNING STARTUP SCREEN > **

End Class