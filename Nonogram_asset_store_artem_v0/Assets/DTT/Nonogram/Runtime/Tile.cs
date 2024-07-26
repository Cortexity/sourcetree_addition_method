using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Holds the information of a tile on the grid.
    /// </summary>
    public class Tile 
    {
        /// <summary>
        /// The correct state according the the current loaded Nonogram configuration.
        /// </summary>
        public Color CorrectStatus { get; private set; }

        /// <summary>
        /// The current state of the tile.
        /// </summary>
        public Color CurrentStatus { get; private set; }

        /// <summary>
        /// The current mark of the tile.
        /// </summary>
        public TileMark CurrentMark { get; private set; }

        /// <summary>
        /// The default color to use on the tiles.
        /// </summary>
        private Color defaultColor;

        /// <summary>
        /// The color the tiles will toggle to when using toggle input.
        /// </summary>
        private Color toggleColor;

        public void Initialize(Color defaultColor, Color toggleColor)
        {
            this.defaultColor = defaultColor;
            this.toggleColor = toggleColor;
        }

        /// <summary>
        /// Sets the status of the tile.
        /// </summary>
        /// <param name="newStatus">The new status for the tile.</param>
        public void ChangeStatus(Color newStatus)
        {
            CurrentMark = TileMark.EMPTY;
            CurrentStatus = newStatus;
        }

        /// <summary>
        /// Toggles the tile between black/white.
        /// </summary>
        /// <param name="newStatus">The new status for the tile.</param>
        public void ToggleStatus(bool newStatus)
        {
            CurrentMark = TileMark.EMPTY;
            CurrentStatus = newStatus ? toggleColor : defaultColor;
        }

        /// <summary>
        /// Changes the mark of the tile.
        /// </summary>
        /// <param name="newMark">The new mark.</param>
        public void ChangeMark(TileMark newMark) => CurrentMark = newMark;

        /// <summary>
        /// Sets the correct status of the tile.
        /// </summary>
        /// <param name="status">The value the tile needs to be for it to be correct.</param>
        public void SetCorrectStatus(Color status) => CorrectStatus = status;
    }
}