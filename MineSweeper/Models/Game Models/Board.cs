
namespace MineSweeper.Models.Game_Models
{
    /// <summary>
    /// A basic model for a disboard. Contains number of rows and columns for the board and a 2D array of Cells.
    /// </summary>
    public class Board
    {
        //-----------------------------------------------------------------------------
        // START OF PROPERTIES
        //-----------------------------------------------------------------------------
        private int numRows;
        private int numColumns;
        public int difficulty {  get; private set; }
        public Cell[,] Cells { get; set; }
        //-----------------------------------------------------------------------------
        // END OF PROPERTIES
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF CONSTRUCTORS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Overloaded constructor for the Board class.
        /// </summary>
        /// <param name="numrows">The number of rows for the disboard</param>
        /// <param name="numcols">The number of columns for the disboard</param>
        public Board(int numrows, int numcols, int gameDifficulty)
        {
            // Setting up the rows and columns
            NumRows = numrows;
            numColumns = numcols;

            difficulty = gameDifficulty;

            Cells = new Cell[numrows, numcols];

            Cells = new Cell[numrows, numcols];

            // Generating a new array of cells for the board. They do not have values yet.
            for ( int r = 0; r < numrows; r++)
            {
                for (int c = 0; c < numcols; c++)
                {
                    Cells[r, c] = new Cell();
                }
            }
            
        }

        //-----------------------------------------------------------------------------
        // END OF CONSTRUCTORS
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF GETTERS AND SETTERS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Ensuring that the number of rows cannot be negative. Will clamp to 0
        /// </summary>
        public int NumRows
        {
            get { return numRows; }
            set
            {
                if (value < 0) numRows = 0;
                else numRows = value;
            }
        }

        /// <summary>
        /// Ensuring that the number of columns cannot be negative. Will clamp to 0
        /// </summary>
        public int NumColumns
        {
            get { return numColumns; }
            set
            {
                if (value < 0) numColumns = 0;
                else numColumns = value;
            }
        }


        //-----------------------------------------------------------------------------
        // END OF GETTERS AND SETTERS
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF METHODS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Will update the cell if and only if the row and col passed in are in boinds of disboard.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="cell"></param>
        public void UpdateCell(int row, int col, Cell cell)
        {
            // Negative validation. Is the rows and column passed in out of bounds.
            if (row < 0 || row > numRows || col < 0 || col > numColumns) return;
            // Update the cell
            Cells[row, col] = cell;
        }

        //-----------------------------------------------------------------------------
        // END OF METHODS
        //-----------------------------------------------------------------------------
    }
}
