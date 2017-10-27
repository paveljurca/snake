## INTRO

	* Nokia :)

	* arrow keys
	* SPACE to pause

	* 3 levels
		* level #1 and #2 => walls
		* level #2 => you go through borders
	* bonus food
		* adds half the score

	* as you eat the snake goes faster


## TODO
	> changing score via Observer/Observable pattern
	> implement the game space as a TableLayoutPanel
		> row and column as index
		> game element as special subclass of Control
			> custom margin, name etc.

        > I guess implement it in XNA framework (VB.NET's not officially supported)


## CODE
	> System.Windows.Forms.Control object

Windows.Forms can't handle tons of moving Control objects

what it was designed for

so they blink

	> every grid of game space has a so-called 1px "margin.All"
        > calling 'quit' method on a SnakeGame instance issues ApplicationExitCall
	
        > written in VisualBasic.NET
		> Microsoft Visual Studio 2010 Express
		> framework .NETv3.5

