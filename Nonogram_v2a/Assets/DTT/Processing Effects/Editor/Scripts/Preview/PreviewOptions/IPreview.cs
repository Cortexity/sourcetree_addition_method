using UnityEngine;

namespace DTT.ProcessingEffects.Editor
{
    /// <summary>
    /// An interface for displaying options in the editor UI.
    /// </summary>
    public interface IPreview
    {
        /// <summary>
        /// Draws in the options of the interface.
        /// </summary>
        public void DrawOptions();

        /// <summary>
        /// Generates an image based on the interface options.
        /// </summary>
        /// <param name="texture">The texture to apply the options on.</param>
        /// <returns>A texture with the options applied to it.</returns>
        public Texture2D GenerateImage(Texture2D texture);
    }
}