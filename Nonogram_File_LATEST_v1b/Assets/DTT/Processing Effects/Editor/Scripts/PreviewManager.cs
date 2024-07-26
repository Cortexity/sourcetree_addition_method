using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using DTT.PublishingTools;

namespace DTT.ProcessingEffects.Editor
{
    [DTTHeader("dtt.processing-effects")]
    public class PreviewManager : DTTEditorWindow
    {
        /// <summary>
        /// Opens this window.
        /// </summary>
        [MenuItem("Tools/DTT/Processing Effects/Processing Effects Window")]
        private static void OpenPreview() => GetWindow<PreviewManager>(false, "Texture effects");

        //general
        /// <summary>
        /// The current loaded nonogram preview.
        /// </summary>
        private Texture2D _generatedTexture;

        /// <summary>
        /// The image to posterize.
        /// </summary>
        private Texture2D _selectedTexture2d;

        /// <summary>
        /// Posterize preview option.
        /// </summary>
        private PosterizePreview _posterize;

        /// <summary>
        /// Pixelate preview option.
        /// </summary>
        private PixelatePreview _pixelate;

        //colors
        /// <summary>
        /// Toggles the advanced options.
        /// </summary>
        private bool _showColorSettings;

        /// <summary>
        /// Defines whether to use colors or not.
        /// </summary>
        private bool _colorsEnabled = true;

        /// <summary>
        /// Multiplies the color output.
        /// </summary>
        private float _colorMultiplier = 1;

        /// <summary>
        /// Sets the threshold of whether to use a pixel or not.
        /// </summary>
        private float _grayscaleCutoff = 1f;

        /// <summary>
        /// Sets the threshold of whether to use a pixel or not.
        /// </summary>
        private float _grayscaleColor;

        //save
        /// <summary>
        /// The name of the file that has been saved.
        /// </summary>
        private string _saveFileName = "newTexture";

        /// <summary>
        /// The save filepath.
        /// </summary>
        private string _filePath;

        /// <summary>
        /// The size the new preview will be in.
        /// </summary>
        private Vector2Int _saveSize = new Vector2Int(1920, 1080);

        //info
        /// <summary>
        /// Instance created to show in UI.
        /// </summary>
        private ColorsUsed _colorsUsed;

        /// <summary>
        /// Adjusts the UI to make it look better.
        /// </summary>
        private UIAdjuster _uiAdjuster;

        /// <summary>
        /// Creates a new ColorsUsed instance. Updates the preview image.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            _uiAdjuster = new UIAdjuster();
            _colorsUsed = new ColorsUsed();
            
            _posterize = new PosterizePreview();
            _pixelate = new PixelatePreview();

            _filePath = Application.dataPath;

            UpdateImage();
        }

        /// <summary>
        /// Renders the UI.
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();

            GUILayout.Space(10);
            EditorGUI.BeginChangeCheck();
            DrawOptions();

            GUILayout.Space(10);
            RenderImageUi();
            if (EditorGUI.EndChangeCheck())
                UpdateImage();

            PreviewObject();
            DrawSaveOptions();

            _colorsUsed.DrawUsedColors();
        }

        /// <summary>
        /// Renders the UI of the image window.
        /// </summary>
        public void RenderImageUi() => _selectedTexture2d =
            (Texture2D)EditorGUILayout.ObjectField(new GUIContent("Texture", "Make sure to enable read/write and set format to RGB"), _selectedTexture2d, typeof(Texture2D), false);

        /// <summary>
        /// Renders to options into the UI.
        /// </summary>
        private void DrawOptions()
        {
            // Color settings
            _showColorSettings = EditorGUILayout.Foldout(_showColorSettings, "Color settings");

            if (_showColorSettings)
            {
                _colorsEnabled = _uiAdjuster.ToggleSpaceRight("Use colors", "Uses color when true and Uses black & white when false.", _colorsEnabled);
                _colorMultiplier = _uiAdjuster.SliderSpaceRight("Color intensity", "Makes the color look darker/brighter.", _colorMultiplier, 0, 3);

                _grayscaleCutoff = _uiAdjuster.SliderSpaceRight("Grayscale threshold", "Sets the grayscale threshold to ignore all grayscales below this value.", _grayscaleCutoff, 0, 1);

                _grayscaleColor = 1;
                if (_colorsEnabled)
                    _grayscaleColor -= _uiAdjuster.SliderSpaceRight("Grayscale color", "Sets the grayscale color to gray out colors.", _grayscaleColor, 0, 1);
            }

            // Interface options
            _posterize.DrawOptions();
            _pixelate.DrawOptions();
        }

        /// <summary>
        /// Renders the image to the UI.
        /// </summary>
        public void PreviewObject()
        {
            GUILayout.Space(10);
            if (_generatedTexture != null)
            {
                // Preview
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Box(_generatedTexture);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
        }

        /// <summary>
        /// Draws the settings in in UI.
        /// </summary>
        private void DrawSaveOptions()
        {
            _saveSize = EditorGUILayout.Vector2IntField(new GUIContent("Resolution", "The size(pixels) the saved preview will be."), _saveSize);
            _saveFileName = EditorGUILayout.TextField(new GUIContent("File name", "The new file will have this name."), _saveFileName);

            if (GUILayout.Button(new GUIContent(_filePath, "The save file location.")))
            {
                _filePath = EditorUtility.OpenFolderPanel("Save location", _filePath, "name");
                if(_filePath == string.Empty)
                    _filePath = Application.dataPath;
            }
            if (GUILayout.Button(new GUIContent("Open save location", "Opens the save location of the file.")))
                Application.OpenURL("file:///" + _filePath);


            GUILayout.Space(10);

            if (GUILayout.Button(new GUIContent("Save texture", "Overrides any existing files with the same name")))
            {
                Texture2D imageToSave = ApplyModifications(_selectedTexture2d); 
                imageToSave = ResizeHandler.Resize(imageToSave, _saveSize);
                SaveTexture.SaveImageToDisk(imageToSave, _filePath, _saveFileName, _selectedTexture2d);
            }
        }

        /// <summary>
        /// Updates the preview image and sets the distinct color into a grid.
        /// </summary>
        public void UpdateImage()
        {
            if (_selectedTexture2d != null)
            {
                // Scale to max size 200 pixels. Reduces processing load.
                int width = 200;
                int height = width;
                float originalWidth = _selectedTexture2d.width;
                float originalHeight = _selectedTexture2d.height;

                if (originalWidth > originalHeight)
                    height = (int)(width * (originalHeight / originalWidth));
                else
                    width = (int)(height * (originalWidth / originalHeight));

                Texture2D textureToModify = ResizeHandler.Resize(_selectedTexture2d, new Vector2Int(width, height));

                // Texture effects.
                textureToModify = ApplyModifications(textureToModify);

                // If any changes where made to the size, it will scale it back to 200 in the editor UI.
                _generatedTexture = ResizeHandler.Resize(textureToModify, new Vector2Int(width, height));

                _colorsUsed.GiveColors(DistinctColors(_generatedTexture.GetPixels()));
            }
        }

        /// <summary>
        /// Applies the selected modifications to the image.
        /// </summary>
        /// <param name="originalTexture">The image to use for the modifications.</param>
        /// <returns>A new image With all the selected modifications applied to it.</returns>
        private Texture2D ApplyModifications(Texture2D originalTexture)
        {
            if (_pixelate.Enabled)
                originalTexture = _pixelate.GenerateImage(originalTexture);
            if (_posterize.Enabled)
                originalTexture = _posterize.GenerateImage(originalTexture);

            // Turn to black and white.
            if (_colorsEnabled)
            {
                originalTexture = GrayscaleHandler.AddGrayscale(originalTexture, _grayscaleColor);
                originalTexture = GrayscaleHandler.ExcludeGrayscale(originalTexture, _grayscaleCutoff);
            }
            else 
            {
                originalTexture = GrayscaleHandler.ToBlackWhite(originalTexture, _grayscaleCutoff);
            }

            // Multiplies the output color.
            if (_colorMultiplier != 1)
                originalTexture = MultiplyColor(originalTexture);

            return originalTexture;
        }

        /// <summary>
        /// Multiplies the color of the given texture.
        /// </summary>
        /// <param name="texture">The texture to multiply the colors of.</param>
        /// <returns>A texture that is brighter/darker.</returns>
        private Texture2D MultiplyColor(Texture2D texture)
        {
            Color[] pixels = texture.GetPixels();
            Color[] newPixels = pixels.Select(item => item * _colorMultiplier).ToArray();

            Texture2D newTexture = new Texture2D(texture.width, texture.height);
            newTexture.SetPixels(newPixels);
            newTexture.Apply();
            return newTexture;
        }

        /// <summary>
        /// Distincts the given color array.
        /// </summary>
        /// <param name="colorsToFilter">The colors to distinct.</param>
        /// <returns>The colors that are distinct from each other.</returns>
        public IEnumerable<Color> DistinctColors(Color[] colorsToFilter) => colorsToFilter.Cast<Color>().Distinct();
    }
}