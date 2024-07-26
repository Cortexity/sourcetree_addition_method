using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Used to add options for number gen types.
    /// </summary>
    public interface IInputSelection
    {
        /// <summary>
        /// Gets the Nonogram numbers based on the input type.
        /// </summary>
        /// <param name="row">Row/column of colors, used to decide which numbers are displayed.</param>
        /// <param name="deafaultColor">The color used to set the tiles as default.</param>
        /// <param name="toggleColor">The color used to mark the tiles as correct.</param>
        /// <returns>The numbers of a single row/column.</returns>
        NumberContainer GetNonogramSideNumbers(Color[] row, Color deafaultColor, Color toggleColor);

        /// <summary>
        /// Checks if the tile is in the correct state.
        /// </summary>
        /// <param name="tile">The tile that needs to be checked.</param>
        /// <returns></returns>
        bool CheckTileStatus(Tile tile);
    }
}
