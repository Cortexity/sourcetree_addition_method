using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Interface used to create generator types.
    /// </summary>
    public interface INonogramGenerator
    {
        /// <summary>
        /// Generates a Nonogram grid.
        /// </summary>
        /// <param name="config">The Nonogram configuration settings to use.</param>
        /// <returns>A grid based of the Nonogram configuration.</returns>
        Color[] Generate(NonogramConfig config);
    }
}