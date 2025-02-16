
namespace MineSweeper.Models.Game_Models
{
    /// <summary>
    /// A Basic model for a cell on Disboard.
    /// </summary>
    public class Cell
    {
        //-----------------------------------------------------------------------------
        // START OF PROPERTIES
        //-----------------------------------------------------------------------------
        // There is no need to encapsilate changable booleans
        public bool IsVisited;
        public bool IsBomb;
        public bool IsFlagged;
        public bool IsSpecial;
        private int numBombNeighbors;
        //-----------------------------------------------------------------------------
        // END OF PROPERTIES
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // START OF GETTERS AND SETTERS
        //-----------------------------------------------------------------------------

        /// <summary>
        /// Restriced setting the numbers of bombs to only 0 - 8
        /// </summary>
        public int NumBombNeighbors
        {
            get { return numBombNeighbors; }
            set
            {
                // Ensuring that there are between 0 and 8 potential bombs
                if(value < 0 || value > 8) return;
                else numBombNeighbors = value;
            }
        }

        //-----------------------------------------------------------------------------
        // END OF GETTERS AND SETTERS
        //-----------------------------------------------------------------------------
    }
}
