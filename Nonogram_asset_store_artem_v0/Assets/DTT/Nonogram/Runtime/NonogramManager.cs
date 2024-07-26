using DTT.MinigameBase;
using DTT.MinigameBase.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Управляет началом/паузой/завершением нонограммы.
    /// </summary>
    public class NonogramManager : MonoBehaviour, IMinigame<NonogramConfig, NonogramResult>, IFinishedable, IRestartable
    {
        /// <summary>
        /// Последняя использованная конфигурация.
        /// </summary>
        public NonogramConfig LastConfig { get; private set; }

        /// <summary>
        /// Ссылка на gridHandler. Используется для создания сетки для нонограммы.
        /// </summary>
        public GridHandler GridHandler { get; private set; }

        /// <summary>
        /// Указывает текущий тип входа в конфигурации.
        /// </summary>
        public SelectionType InputType { get; private set; }

        /// <summary>
        /// Указывает, поставлена ли игра на паузу.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Указывает, не поставлена ли игра на паузу.
        /// </summary>
        public bool IsGameActive => !IsPaused;

        /// <summary>
        /// Вызывается при завершении уровня.
        /// </summary>
        public event Action Finished;

        /// <summary>
        /// Вызывается при запуске уровня.
        /// </summary>
        public event Action Started;

        /// <summary>
        /// Вызывается при приостановке игры.
        /// </summary>
        public event Action PauseGame;

        /// <summary>
        /// Вызывается при возобновлении игры.
        /// </summary>
        public event Action ResumeGame;

        /// <summary>
        /// Обрабатывает данные на финишном уровне.
        /// </summary>
        public event Action<NonogramResult> Finish;

        /// <summary>
        /// Get called when a hint is used. Takes in the remaining amount of hints.
        /// </summary>
        public event Action<int> HintUsed;

        /// <summary>
        /// Holds the current amount of hints that may be used.
        /// </summary>
        public int CurrentHintCount { get; private set; }
        
        /// <summary>
        /// Holds the max time of the config minus elapst time clamped at 0.
        /// </summary>
        private int _currentScoreTime;

        /// <summary>
        /// The time passed in seconds when the validation button was last pressed.
        /// </summary>
        private int _elapsedTime;

        /// <summary>
        /// Dictionary of all the input types.
        /// </summary>
        private Dictionary<SelectionType, IInputSelection> _numberGenerationType;

        /// <summary>
        /// The current input type used for the Nonogram.
        /// </summary>
        public IInputSelection CurrentInputSelection { get; private set;}

        /// <summary>
        /// Добавляет события на финишный уровень и создает новый экземпляр <see cref=”GridHandler”/>.
        /// </summary>
        private void OnEnable()
        {
            Finished += FinishLevel;
            GridHandler = new GridHandler();

            NumberGenToDict();
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void Pause()
        {
            IsPaused = true;
            PauseGame?.Invoke();
        }

        /// <summary>
        /// Unpauses the game.
        /// </summary>
        public void Continue()
        {
            IsPaused = false;
            ResumeGame?.Invoke();
        }

        /// <summary>
        /// Force finishes the game.
        /// </summary>
        public void ForceFinish() => Finished?.Invoke();

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void Restart() => StartGame(LastConfig);

        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <param name="config">Config used to generate the Nonogram.</param>
        public void StartGame(NonogramConfig config)
        {
            // Gets the basic settings.
            LastConfig = config;
            InputType = config.NonogramSettings.GenerationType == NonogramGenerationType.IMAGE ?
                config.ImageGenerator.InputType : SelectionType.DEFAULT_TOGGLE;

            // Resets the current stats.
            CurrentHintCount = LastConfig.NonogramSettings.Hints.hintAmount;

            // Sets the amount of hints that may be used.
            HintUsed?.Invoke(CurrentHintCount);

            // Gets input selection.
            _numberGenerationType.TryGetValue(InputType, out IInputSelection inputSelection);
            CurrentInputSelection = inputSelection;

            // Starts nonogram generation.
            if (config.GeneratedData.grid == null)
                config.SetGeneratedData(GridHandler.GenerateNonogramData(config, CurrentInputSelection));
            else
                GridHandler.UseLastData(config.GeneratedData);

            Started?.Invoke();
        }

        /// <summary>
        /// Checks if each tile is in the correct state.
        /// </summary>
        /// <param name="tiles">The tiles inside the Nonogram.</param>
        /// <param name="elapsedTime">The total of time elapst this Nonogram.</param>
        /// <returns>True when the Nonogram is complete.</returns>
        public bool ValidateNonogram(Tile[] tiles, int elapsedTime)
        {
            _elapsedTime = elapsedTime;
            _currentScoreTime = LastConfig.NonogramSettings.MaxTime - elapsedTime;

            foreach (Tile item in tiles)
            {
                if (!CurrentInputSelection.CheckTileStatus(item))
                    return false;
            }

            Finished?.Invoke();

            return true;
        }

        /// <summary>
        /// Checks if a hint may be used. 
        /// </summary>
        public void TryUseHint()
        {
            if (CurrentHintCount > 0)
            {
                CurrentHintCount--;
                HintUsed?.Invoke(CurrentHintCount);
            }
        }

        /// <summary>
        /// Calculates the score based on the given max time.
        /// </summary>
        /// <returns>The score in 0 to 10.</returns>
        private int CalculateScore() => Mathf.CeilToInt((float)_currentScoreTime / (float)LastConfig.NonogramSettings.MaxTime * 10);

        /// <summary>
        /// Generates all the possible inputSelections and adds them to the dictionary.
        /// </summary>
        private void NumberGenToDict()
        {
            InputSelectionToggle black = new InputSelectionToggle();
            InputSelectionColor colors = new InputSelectionColor();

            _numberGenerationType = new Dictionary<SelectionType, IInputSelection>();

            _numberGenerationType.Add(SelectionType.DEFAULT_TOGGLE, black);
            _numberGenerationType.Add(SelectionType.SELECT_COLOR, colors);
        }

        /// <summary>
        /// Called when all tiles are in the correct state.
        /// </summary>
        private void FinishLevel()
        {
            NonogramResult levelResult = new NonogramResult(_elapsedTime, CalculateScore());
            Finish?.Invoke(levelResult);
        }
    }
}