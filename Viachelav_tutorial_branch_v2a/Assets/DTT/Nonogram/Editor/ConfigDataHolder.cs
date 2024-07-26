using UnityEditor;

namespace DTT.Nonogram
{
    /// <summary>
    /// This class holds the Nonogram configuration for the NonogramConfigEditor.
    /// </summary>
    public class ConfigDataHolder
    {
        /// <summary>
        /// The object to get the data from.
        /// </summary>
        public SerializedObject SerializedObject;

        /// <summary>
        /// This class holds the Nonogram configuration for the NonogramConfigEditor.
        /// </summary>
        /// <param name="obj">The Nonogram configuration to get the data from.</param>
        public ConfigDataHolder(SerializedObject obj)
        {
            SerializedObject = obj;
            GetData();
        }

        // Primary settings.
        /// <summary>
        /// Property field of GeneralSettings.
        /// </summary>
        public SerializedProperty General { get; private set; }

        /// <summary>
        /// Property field of the color settings from the selected config.
        /// </summary>
        public SerializedProperty ColorSettings { get; private set; }

        /// <summary>
        /// Property field of the posterize settings from the selected config.
        /// </summary>
        public SerializedProperty PosterizeSettings { get; private set; }

        /// <summary>
        /// Property field of the generation type.
        /// </summary>
        public SerializedProperty GenerationType { get; private set; }

        /// <summary>
        /// Property field of the image generator, only shown if image is selected in genType.
        /// </summary>
        public SerializedProperty ImageGenerator { get; private set; }

        /// <summary>
        /// Property field of the random generator, only shown if random is selected in genType.
        /// </summary>
        public SerializedProperty RandomGenerator { get; private set; }

        // Image generator
        /// <summary>
        /// The image used for generating the Nonogram.
        /// </summary>
        public SerializedProperty Image { get; private set; }

        /// <summary>
        /// Sets the InputType used for this configuration.
        /// </summary>
        public SerializedProperty InputType { get; private set; }

        // Random generator
        /// <summary>
        /// Sets the difficulty of the configuration, the higher the number the harder it will be.
        /// </summary>
        public SerializedProperty Difficulty { get; private set; }

        /// <summary>
        /// Sets the seed for Nonogram, if left empty it will use a random seed.
        /// </summary>
        public SerializedProperty Seed { get; private set; }

        // Color options.
        /// <summary>
        /// Toggles the use of colors.
        /// </summary>
        public SerializedProperty UseColors { get; private set; }

        /// <summary>
        /// Sets the default color of the tiles.
        /// </summary>
        public SerializedProperty DefaultColors { get; private set; }

        /// <summary>
        /// Sets the toggle color for the toggle input type.
        /// </summary>
        public SerializedProperty ToggleColor { get; private set; }

        /// <summary>
        /// Multiplies the color output.
        /// </summary>
        public SerializedProperty ColorIntensity { get; private set; }

        /// <summary>
        /// The threshold of the grayscale to use a pixel or to ignore it.
        /// </summary>
        public SerializedProperty GrayscaleThreshold { get; private set; }

        /// <summary>
        /// Lerps the color to grayscale.
        /// </summary>
        public SerializedProperty GrayscaleColor { get; private set; }

        // Posterize settings
        /// <summary>
        /// Enables the posterize effect.
        /// </summary>
        public SerializedProperty PosterizeEnabled { get; private set; }

        /// <summary>
        /// The base value of the posterize effect.
        /// </summary>
        public SerializedProperty Level { get; private set; }

        /// <summary>
        /// Gets called when creating an instance. Gets the primarily data of the config.
        /// </summary>
        public void GetData()
        {
            General = SerializedObject.FindProperty("NonogramSettings");
            GenerationType = General.FindPropertyRelative("GenerationType");

            ColorSettings = SerializedObject.FindProperty("ColorSettings");
            PosterizeSettings = SerializedObject.FindProperty("PosterizeSettings");

            ImageGenerator = SerializedObject.FindProperty("ImageGenerator");
            RandomGenerator = SerializedObject.FindProperty("RandomGenerator");

            GetImageGeneratorProperties();
            GetRandomGeneratorProperties();
            GetColorProperties();
            GetPosterizeProperties();
        }

        /// <summary>
        /// Gets the image generator settings.
        /// </summary>
        private void GetImageGeneratorProperties()
        {
            Image = ImageGenerator.FindPropertyRelative("Texture");
            InputType = ImageGenerator.FindPropertyRelative("InputType");
        }

        /// <summary>
        /// Gets the random generator settings.
        /// </summary>
        private void GetRandomGeneratorProperties()
        {
            Difficulty = RandomGenerator.FindPropertyRelative("Difficulty");
            Seed = RandomGenerator.FindPropertyRelative("Seed");
        }

        /// <summary>
        /// Gets the color settings.
        /// </summary>
        private void GetColorProperties()
        {
            UseColors = ColorSettings.FindPropertyRelative("UseColors");
            DefaultColors = ColorSettings.FindPropertyRelative("DefaultColor");
            ToggleColor = ColorSettings.FindPropertyRelative("ToggleColor");
            ColorIntensity = ColorSettings.FindPropertyRelative("ColorIntensity");
            GrayscaleThreshold = ColorSettings.FindPropertyRelative("GrayscaleThreshold");
            GrayscaleColor = ColorSettings.FindPropertyRelative("GrayscaleColor");
        }

        /// <summary>
        /// Gets the posterize settings.
        /// </summary>
        private void GetPosterizeProperties()
        {
            PosterizeEnabled = PosterizeSettings.FindPropertyRelative("PosterizeEnabled");
            Level = PosterizeSettings.FindPropertyRelative("Level");
        }
    }
}
