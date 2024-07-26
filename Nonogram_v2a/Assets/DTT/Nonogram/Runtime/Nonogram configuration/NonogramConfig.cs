using UnityEngine;
namespace DTT.Nonogram
{
    /// <summary>
    /// Configuration of a Nonogram, used to set settings for the Nonogram.
    /// </summary>
    [CreateAssetMenu(fileName ="Nonogram Config", menuName ="DTT/Minigame Nonogram/Nonogram Config")]
    public class NonogramConfig : ScriptableObject
    {
        /// <summary>
        /// The general settings. 
        /// </summary>
        public GeneralConfigSettings NonogramSettings;

        /// <summary>
        /// Gives color options to the Nonogram configuration.
        /// </summary>
        public ColorConfiguration ColorSettings;

        /// <summary>
        /// Gives posterize options to the Nonogram configuration.
        /// </summary>
        public PosterizeConfiguration PosterizeSettings;

        /// <summary>
        /// Used when selected type is IMAGE.
        /// </summary>
        public ImageGenerator ImageGenerator;

        /// <summary>
        /// Used when selected type is RANDOM.
        /// </summary>
        public RandomGenerator RandomGenerator;

        /// <summary>
        /// If the Nonogram was at least once generated, Nonogram manager will use this to render the Nonogram.
        /// </summary>
        public NonogramData GeneratedData { get; private set; }

        /// <summary>
        /// Returns the current generator.
        /// </summary>
        public INonogramGenerator Generator => NonogramSettings.GenerationType == NonogramGenerationType.IMAGE ? (INonogramGenerator)ImageGenerator : RandomGenerator;

        /// <summary>
        /// Sets the saveData.
        /// </summary>
        /// <param name="data">The NonogramData that needs to be saved to this config.</param>
        public void SetGeneratedData(NonogramData data) => GeneratedData = data;
    }
}