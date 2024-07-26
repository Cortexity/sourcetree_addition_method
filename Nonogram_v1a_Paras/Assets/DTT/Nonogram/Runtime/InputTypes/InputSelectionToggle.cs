using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Adds black/white number filter for the nonogram.
    /// </summary>
    public class InputSelectionToggle : IInputSelection
    {
        /// <summary>
        /// Checks if the tile is in the correct state for the toggle input.
        /// </summary>
        /// <param name="tile">The tile that needs to be checked.</param>
        /// <returns>If the tile is in the correct state.</returns>
        public bool CheckTileStatus(Tile tile) => tile.CorrectStatus == tile.CurrentStatus;

        /// <summary>
        /// Gets the nonogram numbers based on the input type.
        /// </summary>
        /// <param name="row">Row/column of colors, used to decide which numbers are displayed.</param>
        /// <param name="deafaultColor">The color used to set the tiles as default.</param>
        /// <param name="toggleColor">The color used to mark the tiles as correct.</param>
        /// <returns>The numbers of a single row/column.</returns>
        public NumberContainer GetNonogramSideNumbers(Color[] row, Color deafaultColor, Color toggleColor)
        {
            List<int> collectedNumbers = new List<int>();
            List<Color> collectedColors = new List<Color>();
            int count = 0;
            Color lastColor = row[0];

            int length = row.Length;
            for (int i = 0; i < length; i++)
            {
                if (row[i] != deafaultColor)
                {
                    count++;
                }
                else
                {
                    if (count != 0)
                    {
                        collectedNumbers.Add(count);
                        collectedColors.Add(Color.black);
                        count = 0;
                    }
                }
            }

            // Adds a number if last tile was on end of the grid or adds 0 if no numbers are present.
            if (count > 0 || collectedNumbers.Count == 0)
            {
                collectedNumbers.Add(count);

                if (lastColor == deafaultColor)
                    collectedColors.Add(Color.black);

                collectedColors.Add(Color.black);
            }

            NumberContainer container = new NumberContainer(collectedNumbers.ToArray(), collectedColors.ToArray());
            return container;
        }
    }
}