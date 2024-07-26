using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace DTT.ProcessingEffects.Editor
{
    /// <summary>
    /// This class draws the given colors in the editor UI. It filters them based on the Hue and then on the Value of the HSV.
    /// </summary>
    public class ColorsUsed
    {
        /// <summary>
        /// The list of colors used in the preview.
        /// </summary>
        private IEnumerable<Color> _usedColors;

        /// <summary>
        /// Opens/closes the view of the colors.
        /// </summary>
        private bool _showList;

        /// <summary>
        /// The scroll location for the color view.
        /// </summary>
        private Vector2 scrollPos;

        /// <summary>
        /// Gives the colors from preview manager to this class.
        /// </summary>
        /// <param name="usedColors">The colors to show under the preview.</param>
        public void GiveColors(IEnumerable<Color> usedColors) => _usedColors = usedColors;

        /// <summary>
        /// Draws in the given colors.
        /// </summary>
        public void DrawUsedColors()
        {
            if (_usedColors == null)
                return;

            IEnumerable<Color> orderedColors = _usedColors
                                                .OrderBy(color => color.GetHue())
                                                .ThenBy(color => color.GetValue());

            GUILayout.Space(10);

            _showList = EditorGUILayout.Foldout(_showList, $"Colors used in preview ({_usedColors.Count<Color>()} colors)");
            if(_showList)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                int count = 0;
                float test = Screen.width;
                int rouned = Mathf.FloorToInt(test / 20 * .8f - 1.5f);
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                foreach (Color item in orderedColors)
                {
                    EditorGUI.DrawRect(EditorGUILayout.GetControlRect(GUILayout.Width(20)), item);
                    count++;
                    if (count > rouned)
                    {
                        GUILayout.EndHorizontal();
                        count = 0;
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(15);
                    }
                }
                GUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            }
        }
    }

    /// <summary>
    /// Extension methods for Color.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Get the H value of HSV.
        /// </summary>
        /// <param name="color">The color to get the value from.</param>
        /// <returns>The H value.</returns>
        public static float GetHue(this Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return h;
        }

        /// <summary>
        /// Get the V value of HSV.
        /// </summary>
        /// <param name="color">The color to get the value from.</param>
        /// <returns>The V value.</returns>
        public static float GetValue(this Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return v;
        }
    }
}