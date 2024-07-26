using UnityEngine;
using UnityEditor;

namespace DTT.ProcessingEffects.Editor
{
    /// <summary>
    /// Enables a pixelated preview for images.
    /// </summary>
    public class PixelatePreview : IPreview
    {
        /// <summary>
        /// Adjusts the UI to make it look better.
        /// </summary>
        private UIAdjuster _uiAdjuster;

        /// <summary>
        /// Toggles the advanced options.
        /// </summary>
        private bool _showAdvancedOptions;

        /// <summary>
        /// The amount of pixels the texture will be converted to.
        /// </summary>
        private int _pixelCount = 10;

        /// <summary>
        /// Enables pixelation on the image.
        /// </summary>
        private bool _pixelateEnabled;

        /// <summary>
        /// Enables pixelation on the image.
        /// </summary>
        public bool Enabled => _pixelateEnabled;

        /// <summary>
        /// Pixelates the given texture.
        /// </summary>
        /// <param name="texture">The texture to pixelate.</param>
        /// <param name="size">The amount of pixels used.</param>
        /// <returns>The texture pixelated.</returns>
        public Texture2D PixelateImage(Texture2D texture) => PixelateHandler.Pixelate(texture, _pixelCount);

        /// <summary>
        /// Draws in the options to display in the editor.
        /// </summary>
        public void DrawOptions()
        {
            _uiAdjuster ??= new UIAdjuster();

            EditorGUILayout.BeginHorizontal();
            _showAdvancedOptions = EditorGUILayout.Foldout(_showAdvancedOptions, "Pixelate options");
            _pixelateEnabled = EditorGUILayout.Toggle(string.Empty, _pixelateEnabled);
            EditorGUILayout.EndHorizontal();

            if (_showAdvancedOptions)
                _pixelCount = _uiAdjuster.IntSliderSpaceRight("Pixels", "Changes the amount of pixels used in the image.", _pixelCount, 2, 100);
        }

        /// <summary>
        /// Generates the image used to show in the editor.
        /// </summary>
        /// <param name="texture">The texture to draw.</param>
        /// <returns>The given texture pixelized.</returns>
        public Texture2D GenerateImage(Texture2D texture) => PixelateImage(texture);
    }
}