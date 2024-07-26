using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Adds posterization options to the Nonogram configuration.
    /// </summary>
    [Serializable]
    public class PosterizeConfiguration 
    {
        /// <summary>
        /// Enables the posterize effect.
        /// </summary>
        public bool PosterizeEnabled;

        /// <summary>
        /// Changes the base value of the posterize effect.
        /// </summary>
        [Range(.1f, 100)]
        public float Level = 1;
    }
}
