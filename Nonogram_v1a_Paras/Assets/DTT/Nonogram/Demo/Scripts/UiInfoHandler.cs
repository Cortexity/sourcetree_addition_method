using DTT.MinigameBase.Timer;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Handles the information the player can see in the UI.
    /// </summary>
    public class UiInfoHandler : MonoBehaviour
    {
        /// <summary>
        /// Used to setup the functions that need to activate when changing the game state.
        /// </summary>
        [SerializeField]
        private NonogramManager _manager;

        /// <summary>
        /// The timer of the game. 
        /// </summary>
        [SerializeField]
        private Timer _timer;

        /// <summary>
        /// The pause menu of the game.
        /// </summary>
        [SerializeField]
        private GameObject _pauseMenu;

        /// <summary>
        /// Text where the user can see his current guess amount.
        /// </summary>
        [SerializeField]
        private Text _currentScoreText;

        /// <summary>
        /// Text where the user can see how many hints are left.
        /// </summary>
        [SerializeField]
        private Text _hintAmountText;

        /// <summary>
        /// The text that displays the timer.
        /// </summary>
        [SerializeField]
        private Text _timerText;

        /// <summary>
        /// Holds the elapst time when last checked for it.
        /// </summary>
        private string _lastCheckTimerString;

        /// <summary>
        /// Object of the hint button.
        /// </summary>
        [SerializeField]
        private GameObject _hintObject;

        /// <summary>
        /// Sets up the events when changes the game state.
        /// </summary>
        private void OnEnable()
        {
            Debug.Log("Enable");
            _manager.Finished += TurnOnPauseMenu;
            if (_timer != null)
            {
                _manager.Finished += _timer.Stop;
                _manager.PauseGame += _timer.Pause;
                _manager.ResumeGame += _timer.Resume;
                _manager.Started += _timer.Begin;
                _manager.Started += StartScoreCounter;
                _manager.Started += SetSettingsToUi;
            }

            _manager.HintUsed += UpdateHintText;
        }

        /// <summary>
        /// Removes the listeners added in OnEnable
        /// </summary>
        
        private void OnDisable()
        {
            Debug.Log("Disable");
            _manager.Finished -= TurnOnPauseMenu;
            if (_timer != null)
            {
                _manager.Finished -= _timer.Stop;
                _manager.PauseGame -= _timer.Pause;
                _manager.ResumeGame -= _timer.Resume;
                _manager.Started -= _timer.Begin;
                _manager.Started -= StartScoreCounter;
                _manager.Started -= SetSettingsToUi;
            }

            _manager.HintUsed -= UpdateHintText;
        }

        /// <summary>
        /// Sets the basic information that the user can see.
        /// </summary>
        public void SetSettingsToUi()
        {
            NonogramConfig config = _manager.LastConfig;
            if (config.NonogramSettings.Hints.useHints && _hintObject != null)
            {
                _hintObject.SetActive(true);
                UpdateHintText(config.NonogramSettings.Hints.hintAmount);
            }
            else
            {
                _hintObject.SetActive(false);
            }
        }

        /// <summary>
        /// Starts the counter to calculate the score.
        /// </summary>
        private void StartScoreCounter() => StartCoroutine(UpdateScoreText());

        /// <summary>
        /// Calculates the current score. Stops the loop when the score reaches zero.
        /// </summary>
        /// <returns>Waits 1 second before calculating score again.</returns>
        private IEnumerator UpdateScoreText()
        {
            yield return new WaitUntil(() => _manager.IsGameActive && _timerText.text != _lastCheckTimerString);
            _lastCheckTimerString = _timerText.text;

            int currentScore = _manager.LastConfig.NonogramSettings.MaxTime - _timer.TimePassed.Seconds;
            UpdateScoreText(currentScore);
            if (currentScore == 0)
                yield break;

            StartCoroutine(UpdateScoreText());
        }

        /// <summary>
        /// Updates the UI of the hints used.
        /// </summary>
        /// <param name="hintsLeft">The amount of hints that may still be used.</param>
        private void UpdateHintText(int hintsLeft) => _hintAmountText.text = hintsLeft.ToString();

        /// <summary>
        /// Updates the guesses text when validating the Nonogram. 
        /// </summary>
        /// <param name="validationsMade">Total amount of times validated.</param>
        private void UpdateScoreText(int validationsMade) => _currentScoreText.text = validationsMade.ToString();

        /// <summary>
        /// Turns on the pause menu.
        /// </summary>
        public void TurnOnPauseMenu() => _pauseMenu.SetActive(true);
    }
}