using MineSweeper.Models;
using MineSweeper.Models.Game_Models;
using System.Drawing;
using System.Text.Json.Serialization;

namespace MineSweeper.BusinessLogic.Game_Logic
{
    /// <summary>
    /// This is the brains for the Mine Sweeper game. Here, ALL the logic will be handled.
    /// </summary>
    public class GameEngine
    {
        //-----------------------------------------------------------------------------
        // START OF VALUES AND PROPERTIES
        //-----------------------------------------------------------------------------

        // Dates for keeping control of the timer in the game 
        private const int minRowCol = 3, maxRowCol = 10;
        private int difficulty { get; set; }
        [JsonInclude]
        private Board disboard { get; set; }
        private bool isRunning { get; set; }
        private int totalNumBombs { get; set; }
        private bool hasSpecial { get; set; }
        private bool hasHitBomb { get; set; }
        private bool isFirstClick { get; set; } = true;

        public int FinalScore { get; set; }
        public int? SavedGameId { get; set; }

        private DateTime startTime { get; set; }

        //-----------------------------------------------------------------------------
        // END OF VALUES AND PROPERTIES
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF CONSTRUCTORS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Existing board game engine
        /// </summary>
        /// <param name="existingBoard"></param>
        public GameEngine(Board existingBoard, int timePlayed)
        {
            isRunning = true;
            hasHitBomb = false;
            this.hasSpecial = false;
            this.disboard = existingBoard; // Use the existing board instead of initializing a new one
            this.difficulty = existingBoard.difficulty; // Make sure difficulty is correctly set
            this.startTime = DateTime.UtcNow - TimeSpan.FromSeconds(timePlayed);
            this.isFirstClick = false; // For loaded games, it's not the first click

            this.totalNumBombs = 0;
            foreach (var cell in disboard.Cells)
            {
                if (cell.IsBomb)
                    this.totalNumBombs++;
            }
        }

        /// <summary>
        /// The default constructor. The game will call the main game loop
        /// </summary>
        public GameEngine(int rows, int columns, int difficulty)
        {
            isRunning = true;
            hasHitBomb = false;
            this.hasSpecial = false;
            this.disboard = new Board(rows, columns, difficulty);
            this.difficulty = difficulty;
            if (startTime == default)
                startTime = DateTime.UtcNow;
        }

        //-----------------------------------------------------------------------------
        // END OF CONSTRUCTORS
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF GETTERS AND SETTERS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Returns if a bomb has been hit.
        /// </summary>
        /// <returns></returns>
        public bool HasHitBomb()
        {
            return hasHitBomb;
        }

        /// <summary>
        /// Will return the state of the board.
        /// </summary>
        /// <returns></returns>
        public Board GetBoardState()
        {
            return disboard;
        }

        /// <summary>
        /// Will get the difficulty level of the game.
        /// </summary>
        /// <returns></returns>
        public int GetDifficulty()
        {
            return difficulty;
        }

        /// <summary>
        /// Returns the total number of bombs.
        /// </summary>
        /// <returns></returns>
        public int GetTotalBombs()
        {
            return this.totalNumBombs;
        }

        public Board GetDisboard()
        {
            return disboard;
        }

        //-----------------------------------------------------------------------------
        // END OF GETTER AND SETTERS
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF METHODS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Will set up the board based off the difficulty level.
        /// The difficulty will determine how many bombs will be distributed and how many specials.
        /// </summary>
        /// <param name="difficulty">difficulty: int - How many bombs and specials will be distributed based off this number.</param>
        private void InitBoard(int firstClickRow, int firstClickCol)
        {
            // Method Vars
            Random rand = new Random();
            int rows = disboard.NumRows;
            int columns = disboard.NumColumns;
            int percentageChanceOfSpawn = difficulty;
            int numOfSpecials = 0;

            // Reset the board
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    disboard.Cells[r, c].IsBomb = false;
                    disboard.Cells[r, c].IsSpecial = false;
                    disboard.Cells[r, c].NumBombNeighbors = 0;
                }
            }

            // Set difficulty values
            switch (difficulty)
            {
                case 1:
                    percentageChanceOfSpawn = 10;
                    numOfSpecials = 3;
                    break;
                case 2:
                    percentageChanceOfSpawn = 8;
                    numOfSpecials = 2;
                    break;
                case 3:
                    percentageChanceOfSpawn = 5;
                    numOfSpecials = 1;
                    break;
            }

            // Create safe zone around first click
            List<Point> safeZone = new List<Point>();
            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    int newRow = firstClickRow + r;
                    int newCol = firstClickCol + c;
                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < columns)
                    {
                        safeZone.Add(new Point(newRow, newCol));
                    }
                }
            }

            // Setting up the bombs
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    // Skip safe zone
                    if (safeZone.Any(p => p.X == r && p.Y == c))
                        continue;

                    if (rand.Next(percentageChanceOfSpawn) < 1)
                    {
                        disboard.Cells[r, c].IsBomb = true;
                        totalNumBombs++;

                        // Update neighbor counts
                        for (int innerRow = -1; innerRow <= 1; innerRow++)
                        {
                            for (int innerColumn = -1; innerColumn <= 1; innerColumn++)
                            {
                                if (innerRow == 0 && innerColumn == 0)
                                    continue;

                                int neighboringRow = r + innerRow;
                                int neighboringCol = c + innerColumn;

                                if (neighboringRow >= 0 && neighboringRow < rows && neighboringCol >= 0 && neighboringCol < columns)
                                    disboard.Cells[neighboringRow, neighboringCol].NumBombNeighbors++;
                            }
                        }
                    }
                }
            }

            // Ensure at least one bomb exists
            if (totalNumBombs == 0)
            {
                // Place one bomb outside safe zone
                List<Point> availableCells = new List<Point>();
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        if (!safeZone.Any(p => p.X == r && p.Y == c))
                        {
                            availableCells.Add(new Point(r, c));
                        }
                    }
                }
                if (availableCells.Count > 0)
                {
                    Point bombPoint = availableCells[rand.Next(availableCells.Count)];
                    disboard.Cells[bombPoint.X, bombPoint.Y].IsBomb = true;
                    totalNumBombs = 1;
                }
            }

            // Place specials
            List<Point> blankLocations = new List<Point>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (!disboard.Cells[row, col].IsBomb && disboard.Cells[row, col].NumBombNeighbors == 0)
                        blankLocations.Add(new Point(row, col));
                }
            }

            // Adding Specials to one of the blank cells randomly
            for (int iterator = 0; iterator < numOfSpecials && blankLocations.Count > 0; iterator++)
            {
                int randomIndex = rand.Next(blankLocations.Count);
                Point point = blankLocations[randomIndex];
                disboard.Cells[point.X, point.Y].IsSpecial = true;
                blankLocations.RemoveAt(randomIndex);
            }
        }

        /// <summary>
        /// Will take action given the row and column of the game board.
        /// </summary>
        /// <param name="selectedRow">row: int - The row on the disboard</param>
        /// <param name="selectedCol">col: int - The column on the disboard</param>
        /// <param name="action">action: int - The choice of what action to take in this cell</param>
        /// <returns>isGameOver: bool - Will return false if the game has ended.</returns>
        public bool UpdateGame(int selectedRow, int selectedCol, int action)
        {
            // Has the cell already been visited
            if (disboard.Cells[selectedRow, selectedCol].IsVisited) return true;

            // Initialize board on first click
            if (isFirstClick && action == 1)
            {
                InitBoard(selectedRow, selectedCol);
                isFirstClick = false;
            }

            // if the player has a special only use it if there is a bomb
            if (action == 1 && disboard.Cells[selectedRow, selectedCol].IsBomb && hasSpecial)
                action = 3;

            // Conducts an action based off the passed in action Integer.
            switch (action)
            {
                case 1: // Visit the cell
                    // Visit the cell
                    //disboard.Cells[selectedRow, selectedCol].IsVisited = true;

                    // Validating if the cell isa a bomb. If so, end the game.
                    if (disboard.Cells[selectedRow, selectedCol].IsBomb)
                    {
                        hasHitBomb = true;
                        return false;
                    }

                    // Checking for Special
                    if (disboard.Cells[selectedRow, selectedCol].IsSpecial)
                    {
                        hasSpecial = true;
                        disboard.Cells[selectedRow, selectedCol].IsVisited = true;
                    }

                    // Checking for numeric cell
                    if (disboard.Cells[selectedRow, selectedCol].NumBombNeighbors > 0)
                    {
                        disboard.Cells[selectedRow, selectedCol].IsVisited = true;
                    }

                    // If the cell is empty, we are going to recursivly clear the rest of the ajacent cells.
                    if (!disboard.Cells[selectedRow, selectedCol].IsBomb && disboard.Cells[selectedRow, selectedCol].NumBombNeighbors == 0 && !disboard.Cells[selectedRow, selectedCol].IsSpecial)
                        RecursiveClearing(selectedRow, selectedCol);
                    break;
                case 2: // Flag the cell
                    disboard.Cells[selectedRow, selectedCol].IsFlagged = !disboard.Cells[selectedRow, selectedCol].IsFlagged;

                    return AllBombsFlagged();

                case 3: // Use special if there is one saved
                    // Using the special
                    hasSpecial = false;

                    // If it is a bomb, we are going to go ahead and flag it
                    if (disboard.Cells[selectedRow, selectedCol].IsBomb)
                    {
                        disboard.Cells[selectedRow, selectedCol].IsFlagged = true;
                        return AllBombsFlagged();
                    }

                    // If the cell is empty, we are going to recursivly clear the rest of the ajacent cells.
                    else if (disboard.Cells[selectedRow, selectedCol].NumBombNeighbors == 0 && !disboard.Cells[selectedRow, selectedCol].IsSpecial)
                        RecursiveClearing(selectedRow, selectedCol);

                    // Is it a special
                    else if (disboard.Cells[selectedRow, selectedCol].IsSpecial)
                    {
                        hasSpecial = true;
                        disboard.Cells[selectedRow, selectedCol].IsVisited = true;
                    }
                    else // Must be numeric
                    {
                        disboard.Cells[selectedRow, selectedCol].IsVisited = true;
                    }

                    break;
            }
            return true;
        }

        /// <summary>
        /// Resursive Minesweeper agorythem.
        /// </summary>
        /// <param name="row">row: int - The row for the cell that we are cunducting a check on.</param>
        /// <param name="col">col: int - The column for the cell that we are cunducting a check on.</param>
        private void RecursiveClearing(int row, int col)
        {
            /**
             * 1) out of bounds
             * 2) Is it a bomb
             * 3) is it numeric
             * 4) is it empty
             */

            // Checking for out of bounds
            if (row < 0 || row >= disboard.Cells.GetLength(0) || col < 0 || col >= disboard.Cells.GetLength(1))
                return;

            // If already checked
            if (disboard.Cells[row, col].IsVisited || disboard.Cells[row, col].IsSpecial) return;

            // Reveal
            disboard.Cells[row, col].IsVisited = true;

            // Is it a bomb
            if (disboard.Cells[row, col].IsBomb)
                return;

            if (disboard.Cells[row, col].NumBombNeighbors == 0)
            {
                // Calling recursion on the 8 surrounding neighbors
                RecursiveClearing(row - 1, col - 1);
                RecursiveClearing(row - 1, col);
                RecursiveClearing(row - 1, col + 1);
                RecursiveClearing(row, col - 1);
                RecursiveClearing(row, col + 1);
                RecursiveClearing(row + 1, col - 1);
                RecursiveClearing(row + 1, col);
                RecursiveClearing(row + 1, col + 1);
            }
        }

        /// <summary>
        /// Checks if all the Bombs are flagged if not it will return true continueing the game 
        /// </summary>
        /// <returns>bComtinueGame: bool - Will return true only if there are more bombs to find.</returns>
        public bool AllBombsFlagged()
        {
            int flaggedBombs = 0;
            int totalBombs = totalNumBombs;
            int incorrectFlags = 0;

            foreach (Cell cell in disboard.Cells)
            {
                if (cell.IsBomb && cell.IsFlagged)
                {
                    flaggedBombs++; // Correctly flagged bomb
                    Console.WriteLine("Correct flag");
                }
                else if (!cell.IsBomb && cell.IsFlagged)
                {
                    incorrectFlags++; // Incorrect flag
                    Console.WriteLine("InCorrect flag");
                }
            }

            // Ensure that ALL bombs are flagged AND there are NO incorrect flags
            return (flaggedBombs == totalBombs) && (incorrectFlags == 0);
        }

        public bool IsGameWin()
        {
            foreach (Cell cell in disboard.Cells)
            {
                if (!cell.IsBomb && !cell.IsVisited)
                    return false;
            }
            return true;
        }

        public int GenerateScore()
        {
            int score = 0;
            foreach (Cell cell in disboard.Cells)
            {
                if (cell.IsVisited)
                    score++;
            }
            
            return score;
        }

        public TimeSpan GetElapsedTime()
        {
            return DateTime.UtcNow - startTime; // Calculate time difference
        }

        //-----------------------------------------------------------------------------
        // END OF METHODS
        //-----------------------------------------------------------------------------
    }
}
