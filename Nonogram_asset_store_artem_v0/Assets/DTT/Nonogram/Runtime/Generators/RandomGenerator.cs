using UnityEngine;
using System;

namespace DTT.Nonogram
{
    /// <summary>
    /// Generates a Nonogram with random correct tiles.
    /// </summary>
    [Serializable]
    public class RandomGenerator : INonogramGenerator
    {
        /// <summary>
        /// Sets the difficulty of the configuration, the higher the number the harder it will be.
        /// </summary>
        [Range(1, 5)]
        public int Difficulty = 3;

        /// <summary>
        /// Sets the seed for Nonogram, if left empty it will use a random seed.
        /// </summary>
        [SerializeField]
        public int Seed;

        /// <summary>
        /// Randomly generates a Nonogram.
        /// </summary>
        /// <param name="config">The Nonogram configuration settings to use.</param>
        /// <returns>A randomly generated Nonogram.</returns>
        public Color[] Generate(NonogramConfig config)
        {
            if (Seed == 0)
                Seed = (int)DateTime.Now.Ticks;

            UnityEngine.Random.InitState(Seed);

            Color[] grid = new Color[config.NonogramSettings.GridSize.x * config.NonogramSettings.GridSize.y];

            int length = grid.Length;
            for (int x = 0; x < length; x++)
                grid[x] = UnityEngine.Random.Range(0, 10) > Difficulty ? config.ColorSettings.ToggleColor : config.ColorSettings.DefaultColor;

            return grid;
        }
    }
}