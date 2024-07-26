using UnityEngine;
using UnityEditor;

namespace DTT.ProcessingEffects.Editor
{
    /// <summary>
    /// Posterize options for the preview manager.
    /// </summary>
    public class PosterizePreview : IPreview
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
        /// The amount of color steps used.
        /// </summary>
        private float _level = 1.14f;

        /// <summary>
        /// Enables posterize on the image.
        /// </summary>
        private bool _posterizeEnabled;

        /// <summary>
        /// Enables pixelation on the image.
        /// </summary>
        public bool Enabled => _posterizeEnabled;

        /// <summary>
        /// Generates a Posterized image based on the given texture and size.
        /// </summary>
        /// <param name="tex">The texture to convert into a posterized texture.</param>
        /// <returns>The image in the given resolution.</returns>
        public Texture2D PosterizeImage(Texture2D tex) => PosterizeHandler.Posterize(tex, _level);

        /// <summary>
        /// Draws in the options to display in the editor.
        /// </summary>
        public void DrawOptions()
        {
            _uiAdjuster ??= new UIAdjuster();

            EditorGUILayout.BeginHorizontal();
            _showAdvancedOptions = EditorGUILayout.Foldout(_showAdvancedOptions, "Posterize options");
            _posterizeEnabled = EditorGUILayout.Toggle(string.Empty, _posterizeEnabled);
            EditorGUILayout.EndHorizontal();

            if (_showAdvancedOptions)
                _level = _uiAdjuster.SliderSpaceRight("Level", "Changes the amount of colors used in the posterization.", _level, .1f, 100);
        }

        /// <summary>
        /// Generates the image used to show in the editor.
        /// </summary>
        /// <param name="tex">The texture to draw.</param>
        /// <returns>The given texture pixelized.</returns>
        public Texture2D GenerateImage(Texture2D tex) => PosterizeImage(tex);
    }
}