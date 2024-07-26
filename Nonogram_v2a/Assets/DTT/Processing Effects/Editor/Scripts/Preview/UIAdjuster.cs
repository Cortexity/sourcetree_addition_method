using UnityEditor;
using UnityEngine;

namespace DTT.ProcessingEffects.Editor
{
    /// <summary>
    /// Adds spacing options for the editor UI.
    /// </summary>
    public class UIAdjuster
    {
        /// <summary>
        /// The default spacing the options will have to the right.
        /// </summary>
        private const int _DEFAULT_SPACING = 15;

        /// <summary>
        /// Creates an EditorGUILayout slider spaced to the right x column.
        /// </summary>
        /// <param name="name">The name to display.</param>
        /// <param name="tooltip">The tooltip for the slider.</param>
        /// <param name="value">The value to modify.</param>
        /// <param name="min">Minimum slider value.</param>
        /// <param name="max">Maximum slider value.</param>
        /// <param name="column">Times spaced to the right.</param>
        public float SliderSpaceRight(string name, string tooltip, float value, float min, float max, int column = 1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(_DEFAULT_SPACING * column);
            float sliderValue;
            sliderValue = EditorGUILayout.Slider(new GUIContent(name, tooltip), value, min, max);
            GUILayout.EndHorizontal();
            return sliderValue;
        }

        /// <summary>
        /// Creates an EditorGUILayout slider spaced to the right x column.
        /// </summary>
        /// <param name="name">The name to display.</param>
        /// <param name="tooltip">The tooltip for the slider.</param>
        /// <param name="value">The value to modify.</param>
        /// <param name="min">Minimum slider value.</param>
        /// <param name="max">Maximum slider value.</param>
        /// <param name="column">Times spaced to the right.</param>
        public int IntSliderSpaceRight(string name, string tooltip, int value, int min, int max, int column = 1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(_DEFAULT_SPACING * column);
            int sliderValue;
            sliderValue = EditorGUILayout.IntSlider(new GUIContent(name, tooltip), value, min, max);
            GUILayout.EndHorizontal();
            return sliderValue;
        }

        /// <summary>
        /// Creates an EditorGUILayout toggle spaced to the right x column.
        /// </summary>
        /// <param name="name">The name to display.</param>
        /// <param name="tooltip">The tooltip for the slider.</param>
        /// <param name="value">The value to modify.</param>
        /// <param name="column">Times spaced to the right.</param>
        public bool ToggleSpaceRight(string name, string tooltip, bool value, int column = 1)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(_DEFAULT_SPACING * column);
            bool toggleValue;
            toggleValue = EditorGUILayout.Toggle(new GUIContent(name, tooltip), value);
            GUILayout.EndHorizontal();
            return toggleValue;
        }
    }
}
