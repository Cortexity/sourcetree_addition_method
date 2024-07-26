using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// All the data needed to render a Nonogram.
    /// </summary>
    public struct NonogramData
    {
        /// <summary>
        /// Holds the grid values of the Nonogram.
        /// </summary>
        public readonly Color[] grid;

        public readonly Vector2Int gridSize;

        /// <summary>
        /// Holds the numbers for the columns.
        /// </summary>
        public readonly NumberContainer[] columns;

        /// <summary>
        /// Holds the numbers for the rows.
        /// </summary>
        public readonly NumberContainer[] rows;

        /// <summary>
        /// Holds all the different colors.
        /// </summary>
        public readonly IEnumerable<Color> differentColors;

        /// <summary>
        /// Holds the input type.
        /// </summary>
        public readonly SelectionType type;

        /// <summary>
        /// The data needed to render a Nonogram.
        /// </summary>
        /// <param name="grid">array that fills in the Nonogram grid.</param>
        /// <param name="gridSize">The dimensions of the Nonogram.</param>
        /// <param name="columns">Array of NumberContainers for the columns.</param>
        /// <param name="rows">Array of NumberContainers for the rows.</param>
        /// <param name="differentColors">Array of all different colors used.</param>
        /// <param name="type">The type of input used.</param>
        public NonogramData(Color[] grid, Vector2Int gridSize, NumberContainer[] columns, NumberContainer[] rows, IEnumerable<Color> differentColors, SelectionType type)
        {
            this.grid = grid;
            this.columns = columns;
            this.rows = rows;
            this.differentColors = differentColors;
            this.type = type;
            this.gridSize = gridSize;
        }
    }
}
