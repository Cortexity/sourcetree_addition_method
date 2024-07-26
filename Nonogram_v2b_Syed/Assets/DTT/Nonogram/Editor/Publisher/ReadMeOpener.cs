#if UNITY_EDITOR

using DTT.PublishingTools;
using UnityEditor;

namespace DTT.MinigameNonogram.Editor
{
    /// <summary>
    /// Class that handles opening the editor window for the Nonogram package.
    /// </summary>
    internal static class ReadMeOpener 
    {
        /// <summary>
        /// Opens the readme for this package.
        /// </summary>
        [MenuItem("Tools/DTT/Nonogram/ReadMe")]
        private static void OpenReadMe() => DTTEditorConfig.OpenReadMe("dtt.nonogram");
    }
}
#endif