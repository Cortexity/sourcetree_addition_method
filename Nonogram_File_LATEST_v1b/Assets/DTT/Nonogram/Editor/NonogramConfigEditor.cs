using UnityEngine;
using UnityEditor;
using DTT.Nonogram;
using DTT.PublishingTools;
using System.Collections.Generic;
using System.Linq;
using DTT.ProcessingEffects.Editor;
using DTT.ProcessingEffects;

namespace DTT.Nonogram.Editor
{
    /// <summary>
    /// This class overrides the default inspector for the Nonogram config and adds in more options to adjust.
    /// </summary>
    [DTTHeader("dtt.nonogram", "Nonogram configuration")]
    [CustomEditor(typeof(NonogramConfig), true)]
    public class NonogramConfigEditor : DTTInspector
    {
        /// <summary>
        /// Holds the data of the selected config.
        /// </summary>
        private ConfigDataHolder _data;

        /// <summary>
        /// Stores the last generated Nonogram image.
        /// </summary>
        private Texture2D _lastGeneration;

        /// <summary>
        /// The last selected Nonogram configuration.
        /// </summary>
        private NonogramConfig _selectedConfig;

        /// <summary>
        /// Folds out the generator options.
        /// </summary>
        private bool _generatorFoldout = true;

        /// <summary>
        /// Folds out the color options.
        /// </summary>
        private bool _colorFoldout;

        /// <summary>
        /// Folds out the posterize options.
        /// </summary>
        private bool _posterizeFoldout;

        /// <summary>
        /// Instance to show the used colors at the bottom of the config.
        /// </summary>
        private ColorsUsed _colorDisplay;

        /// <summary>
        /// Holds the last time the UI image has been done.
        /// </summary>
        private float _lastGenTime;

        /// <summary>
        /// The default spacing used for the childs of the foldout UI.
        /// </summary>
        private const float _DEFAULT_SPACING = 15;

        /// <summary>
        /// Time before another change check can be done. Gives a little more room for performance.
        /// </summary>
        private const float _CHANGE_CHECK_STEP = .5f;

        /// <summary>
        /// Handles the rendering of the ui.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            NonogramConfig script = (NonogramConfig)target;

            if (_selectedConfig != script && script != null)
            {
                _selectedConfig = script;
                _data = new ConfigDataHolder(serializedObject);
                _colorDisplay = new ColorsUsed();
            }
            else if (_data != null)
            {
                EditorGUI.BeginChangeCheck();
                DrawProperties();
                serializedObject.ApplyModifiedProperties();
                if(Time.time + _CHANGE_CHECK_STEP > _lastGenTime)
                {
                    _lastGenTime = Time.time;
                    PrevieNonogram(script);
                }
            }
        }

        /// <summary>
        /// Handles rendering the properties.
        /// </summary>
        private void DrawProperties()
        {
            EditorGUILayout.PropertyField(_data.General);

            switch (_data.GenerationType.enumValueIndex)
            {
                case 0:
                    DrawImageGeneratorOptions();
                    break;
                case 1:
                    DrawRandomGeneratorOptions();
                    break;
            }

            DrawColorOptions();
            if(_data.GenerationType.enumValueIndex == 0)
                DrawPosterizeOptions();
        }

        /// <summary>
        /// Draws in the image generator options.
        /// </summary>
        private void DrawImageGeneratorOptions()
        {
            _generatorFoldout = EditorGUILayout.Foldout(_generatorFoldout, "Image generator settings");
            if (!_generatorFoldout)
                return;

            DrawPropertySpacedToRight(_data.Image, 1);
            DrawPropertySpacedToRight(_data.InputType, 1);

            EditorGUILayout.Space(10);
        }

        /// <summary>
        /// Draws in the random generator options.
        /// </summary>
        private void DrawRandomGeneratorOptions()
        {
            _generatorFoldout = EditorGUILayout.Foldout(_generatorFoldout, "Random generator settings");
            if (!_generatorFoldout)
                return;

            DrawPropertySpacedToRight(_data.Difficulty, 1);
            DrawPropertySpacedToRight(_data.Seed, 1);

            EditorGUILayout.Space(10);
        }

        /// <summary>
        /// Draws in the color options of the config.
        /// </summary>
        private void DrawColorOptions()
        {
            _colorFoldout = EditorGUILayout.Foldout(_colorFoldout, "Color settings");
            if (!_colorFoldout)
                return;
            if (_data.GenerationType.enumValueIndex == 0 && _data.InputType.enumValueIndex == 1)
                DrawPropertySpacedToRight(_data.UseColors, 1);
            DrawPropertySpacedToRight(_data.DefaultColors, 1);
            if(_data.GenerationType.enumValueIndex == 1 || _data.InputType.enumValueIndex == 0)
                DrawPropertySpacedToRight(_data.ToggleColor, 1);
            if (_data.GenerationType.enumValueIndex == 0)
            {
                DrawPropertySpacedToRight(_data.ColorIntensity, 1);
                DrawPropertySpacedToRight(_data.GrayscaleThreshold, 1);
            }
            if(_data.GenerationType.enumValueIndex == 0 && _data.InputType.enumValueIndex == 1 && _data.UseColors.boolValue)
                DrawPropertySpacedToRight(_data.GrayscaleColor, 1);

            EditorGUILayout.Space(10);
        }

        /// <summary>
        /// Draws in the posterize options of the config.
        /// </summary>
        private void DrawPosterizeOptions()
        {
            EditorGUILayout.BeginHorizontal();
            _posterizeFoldout = EditorGUILayout.Foldout(_posterizeFoldout, "Posterize settings");
            _data.PosterizeEnabled.boolValue = EditorGUILayout.Toggle(_data.PosterizeEnabled.boolValue);
            EditorGUILayout.EndHorizontal();
            if (!_posterizeFoldout)
                return;

            DrawPropertySpacedToRight(_data.Level, 1);

            EditorGUILayout.Space(10);
        }

        /// <summary>
        /// Draws in the properties spaced to the right.
        /// </summary>
        /// <param name="property">The property to draw.</param>
        /// <param name="column">The amount of spaces to the right.</param>
        private void DrawPropertySpacedToRight(SerializedProperty property, int column)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(_DEFAULT_SPACING * column);
            EditorGUILayout.PropertyField(property);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Handles rendering the preview button and result of the Nonogram preview.
        /// </summary>
        /// <param name="config">The config used for the Nonogram.</param>
        private void PrevieNonogram(NonogramConfig config)
        {
            GUILayout.Space(10);

            Vector2Int gridSize = config.NonogramSettings.GridSize;

            if (gridSize.x < 2 || gridSize.y < 2)
                return;

            Color[] previewGeneration = config.Generator.Generate(config);

            if (previewGeneration != null && previewGeneration.Length > 0)
                GenerateTexture(gridSize, previewGeneration);

            if (_lastGeneration != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Box(_lastGeneration);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                _colorDisplay.DrawUsedColors();
            }
        }

        /// <summary>
        /// Generates a texture based on the given color[,] and resizes it to be 200/200.
        /// </summary>
        /// <param name="nonoramSize">The size of the Nonogram in tiles.</param>
        /// <param name="grid">The grid that gets turned into a texture.</param>
        private void GenerateTexture(Vector2Int nonoramSize, Color[] grid)
        {
            // Calculates the sizes of each axis.
            int windowWidth = 200;
            int windowHeight = 200;

            if (nonoramSize.x > nonoramSize.y)
                windowHeight = (int)(windowWidth * (float)(nonoramSize.y / (float)nonoramSize.x));
            else
                windowWidth = (int)(windowHeight * (float)(nonoramSize.x / (float)nonoramSize.y));

            Texture2D tex = new Texture2D(nonoramSize.x, nonoramSize.y);
            tex.SetPixels(grid);
            tex.Apply();
            tex = ResizeHandler.Resize(tex, new Vector2Int(windowWidth, windowHeight));
            _lastGeneration = tex;

            _colorDisplay.GiveColors(DistinctColors(_lastGeneration.GetPixels()));
        }

        /// <summary>
        /// Distincts the given color array.
        /// </summary>
        /// <param name="colorsToFilter">The colors to distinct.</param>
        /// <returns>The colors that are distinct from each other.</returns>
        public IEnumerable<Color> DistinctColors(Color[] colorsToFilter) => colorsToFilter.Cast<Color>().Distinct();
    }
}
