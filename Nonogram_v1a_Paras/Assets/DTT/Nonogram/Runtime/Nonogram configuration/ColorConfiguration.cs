using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Grants color options to the Nonogram configuration.
    /// </summary>
    [Serializable]
    public class ColorConfiguration
    {
        /// <summary>
        /// Enables the use of colors in the Nonogram.
        /// </summary>
        public bool UseColors = true;

        /// <summary>
        /// Sets the default color of the Nonogram.
        /// </summary>
        public Color DefaultColor = Color.white;

        /// <summary>
        /// Sets the color for the toggles tiles when using toggle input.
        /// </summary>
        public Color ToggleColor = Color.black;

        /// <summary>
        /// Multiplies the color output.
        /// </summary>
        [Range(0,2)]
        public float ColorIntensity = 1;

        /// <summary>
        /// The threshold of the grayscale to use a pixel or to ignore it.
        /// </summary>
        [Range(0,1)]
        public float GrayscaleThreshold = 1;

        /// <summary>
        /// Lerps the color to grayscale.
        /// </summary>
        [Range(0, 1)]
        public float GrayscaleColor = 1;
    }
}
