using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Generates the NonogramData, color of tiles and the numbers on the sides. This is based on the config given.
    /// </summary>
    public class GridHandler
    {
        /// <summary>
        /// Gets called when done generating Nonogramdata.
        /// </summary>
        public event Action<NonogramData> NonogramDataGenerated;

        /// <summary>
        /// Generates a new NonogramData and gets numbers for the sides and adds it to the data
        /// </summary>
        /// <param name="config">The configuration used to generate the Nonogram./param>
        /// <param name="inputSelection">Used to get the numbers on the sides./param>
        /// <return>A generated Nonogram.</return>>
        public NonogramData GenerateNonogramData(NonogramConfig config, IInputSelection inputSelection)
        {
            Vector2Int gridSize = config.NonogramSettings.GridSize;
            bool useColors = config.ColorSettings.UseColors && config.ImageGenerator.InputType != SelectionType.DEFAULT_TOGGLE;

            SelectionType autoColor = config.ImageGenerator.InputType;

            Color[] grid = config.Generator.Generate(config);

            int xCount = gridSize.x;
            int yCount = gridSize.y;

            List<NumberContainer> columns = new List<NumberContainer>();
            List<NumberContainer> rows = new List<NumberContainer>();

            Color defaultColor = config.ColorSettings.DefaultColor;
            Color toggleColor = config.ColorSettings.ToggleColor;

            // Top side numbers.
            for (int posX = 0; posX < xCount; posX++)
            {
                Color[] eacRow = new Color[yCount];
                for (int posY = 0; posY < yCount; posY++)
                    eacRow[posY] = grid[posX + (posY * xCount)];

                rows.Add(inputSelection.GetNonogramSideNumbers(eacRow, defaultColor, toggleColor));
            }

            // Left side numbers
            for (int posY = 0; posY < yCount; posY++)
            {
                Color[] eachcolumn = new Color[xCount];
                for (int posX = 0; posX < xCount; posX++)
                    eachcolumn[posX] = grid[(posY * xCount) + posX];

                Array.Reverse(eachcolumn);
                columns.Add(inputSelection.GetNonogramSideNumbers(eachcolumn, defaultColor, toggleColor));
            }

            IEnumerable<Color> distinctColors = DistinctColors(grid);
            SelectionType type = config.NonogramSettings.GenerationType == NonogramGenerationType.RANDOM ? SelectionType.DEFAULT_TOGGLE : config.ImageGenerator.InputType;
            NonogramData data = new NonogramData(grid, gridSize, columns.ToArray(), rows.ToArray(), distinctColors, type);

            NonogramDataGenerated?.Invoke(data);
            return data;
        }

        /// <summary>
        /// Gets called instead of GenerateNonogramData if the NonogramData already exists.
        /// </summary>
        /// <param name="data">The previous generated data.</param>
        public void UseLastData(NonogramData data) => NonogramDataGenerated?.Invoke(data);

        /// <summary>
        /// Collects all different colors in the nonogram.
        /// </summary>
        /// <param name="colorsToFilter">List of colors to distinct</param>
        /// <returns>All distinct colors.</returns>
        public IEnumerable<Color> DistinctColors(Color[] colorsToFilter) => colorsToFilter.Cast<Color>().Distinct();
    }
}