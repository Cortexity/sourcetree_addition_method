using UnityEngine;

namespace DTT.MinigameBase.LevelSelect
{
    /// <summary>
    /// Base operations for using the <see cref="LevelSelectHandler{TConfig,TResult,TMinigame}"/>.
    /// </summary>
    [System.Serializable]    
    
    public abstract class LevelSelectHandlerBase : MonoBehaviour
    {
        /// <summary>
        /// Retrieves the current level.
        /// </summary>
        public abstract int CurrentLevel { get; }
        
        /// <summary>
        /// Hides the level selection user interface.
        /// </summary>
        public abstract void HideLevelSelect();
        
        /// <summary>
        /// Shows the level selection user interface.
        /// </summary>
        public abstract void ShowLevelSelect();
        public abstract void ShowLevelNextDoneHide();


        public abstract void Nextss();
    }
}