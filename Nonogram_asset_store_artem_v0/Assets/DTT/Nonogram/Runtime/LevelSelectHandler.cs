using UnityEngine;
using DTT.MinigameBase.LevelSelect;
using System.Collections.ObjectModel;
using System;

namespace DTT.Nonogram
{
    /// <summary>
    /// Handles the level selection/score. Communication between Nonogram and Minigame Base
    /// </summary>
    public class LevelSelectHandler : LevelSelectHandler<NonogramConfig, NonogramResult, NonogramManager>
    {
        /// <summary>
        /// The initial amount of levels that will show up when creating the database.
        /// </summary>
        private const int INITIAL_LEVELS = 60;

        /// <summary>
        /// The level data inside of the database.
        /// </summary>
        public ReadOnlyCollection<NonogramConfig> Data => Array.AsReadOnly(_nonoData);

        /// <summary>
        /// The level data inside of the database.
        /// </summary>
        [SerializeField]
        [Tooltip("The level data inside of the database.")]
        private NonogramConfig[] _nonoData = new NonogramConfig[INITIAL_LEVELS];

        /// <summary>
        /// Should calculate a score between 0 and 1, of how the user performed based on the results.
        /// </summary>
        /// <param name="result">The result of how the user performed.</param>
        /// <returns>A score between 0 and 1.</returns>
        protected override float CalculateScore(NonogramResult result) => result.timesChecked;

        /// <summary>
        /// Should retrieve the level config object for that specific level.
        /// </summary>
        /// <param name="levelNumber">The current level that a config is required for.</param>
        /// <returns>The level config that is required for this level.</returns>
        protected override NonogramConfig GetConfig(int levelNumber) => Data[(int)Mathf.Clamp(levelNumber - 1, 0, Mathf.Infinity)];
    }
}
