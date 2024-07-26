using System;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// The general configuration settings.
    /// </summary>
    [Serializable]
    public class GeneralConfigSettings
    {
        /// <summary>
        /// Defines the size of the grid, default = 10,10.
        /// </summary>
        public Vector2Int GridSize = new Vector2Int(10, 10);

        /// <summary>
        /// The time before the score reaches 0.
        /// </summary>
        public int MaxTime = 600;

        /// <summary>
        /// Hint options.
        /// </summary>
        public HintConfig Hints;

        /// <summary>
        /// Defines the generation type.
        /// </summary>
        public NonogramGenerationType GenerationType;
    }
}