using DTT.MinigameBase.LevelSelect.PaginationManagement;
using DTT.MinigameBase.UI;
using DTT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DTT.MinigameBase.LevelSelect
{
    /// <summary>
    /// The entry point for level selection in minigames.
    /// </summary>
    public class LevelSelect : MonoBehaviour
    {
        public static LevelSelect instance;
        /// <summary>
        /// Name of the Level Select scene environment.
        /// </summary>
        public const string SCENE_NAME = "DTT Level Select";

        /// <summary>
        /// Called when a level is selected, passing its Level Data.
        /// </summary>
        public event Action<LevelData> LevelSelected;

        /// <summary>
        /// The level that is currently selected.
        /// </summary>
        public LevelButton SelectedLevel { get; private set; }

        /// <summary>
        /// The pagination that is used for displaying the levels in pages.
        /// </summary>
        [SerializeField]
        [Tooltip("The pagination that is used for displaying the levels in pages.")]
        private Pagination _pagination;

        /// <summary>
        /// The prefab of a button that displays a level status.
        /// </summary>
        [SerializeField]
        [Tooltip("The prefab of a button that displays a level status.")]
        public LevelButton _levelButtonPrefab;

        /// <summary>
        /// The transform parent of where the buttons reside.
        /// </summary>
        [SerializeField]
        [Tooltip("The transform parent of where the buttons reside.")]
        public Transform _buttonsParent;

        /// <summary>
        /// The database that stores all the level data.
        /// </summary>
        [SerializeField]
        [Tooltip("The database that stores all the level data.")]
        public LevelDatabase _database;

        /// <summary>
        /// All the active instances of buttons inside the level select.
        /// </summary>
        public List<LevelButton> _levelButtonInstances = new List<LevelButton>();

        /// <summary>
        /// Populates the Level Select environment with a given level database.
        /// </summary>
        /// <param name="database">The database for which to make the Level Select environment.</param>
        public void Populate(LevelDatabase database)
        {
            _database = database;

            _pagination.Populate(Mathf.CeilToInt(_database.LevelCount / (float)15), 15);
            DisplayPageLevels(_pagination.CurrentPage);
        }

        private void Awake()
        {
            instance = this;
        }
        /// <summary>
        /// Adds listeners.
        /// </summary>
        private void OnEnable() => _pagination.PageChanged += DisplayPageLevels;

        /// <summary>
        /// Removes listeners.
        /// </summary>
        private void OnDisable() => _pagination.PageChanged += DisplayPageLevels;

        /// <summary>
        /// Displays the levels for the given page number.
        /// </summary>
        /// <param name="pageNumber">The number of the page to show the levels of.</param>
        private void DisplayPageLevels(int pageNumber)
        {
            // Disable controls during transition.
            _pagination.ControlsAreEnabled = false;

            // Clean up existing levels.
            ClearLevels();

            // Add levels for current page.
            AddLevels();
        }

        /// <summary>
        /// Adds in the levels to the current page.
        /// </summary>
        private void AddLevels()
        {
            for (int i = 0; i < _pagination.ItemsPerPage; i++)
            {
                if (i == _database.LevelCount - _pagination.CurrentPage * _pagination.ItemsPerPage)
                {
                    _pagination.ControlsAreEnabled = true;
                    break;
                }

                LevelButton instance = Instantiate(_levelButtonPrefab, _buttonsParent);
                _levelButtonInstances.Add(instance);

                instance.transform.localScale = Vector3.zero;
                int _i = i;
                DTTween.Value(0, 1, 0.3f, i * 0.05f, Easing.EASE_OUT_BACK,
                    onValueChanged: (float value) => _levelButtonInstances[_i].transform.localScale = Vector3.one * value,
                    onComplete: () =>
                    {
                        _levelButtonInstances[_i].transform.localScale = Vector3.one;
                        if (_i == _pagination.ItemsPerPage - 1)
                            _pagination.ControlsAreEnabled = true;
                    });

                LevelData data = _database.Data[i + _pagination.CurrentPage * _pagination.ItemsPerPage];
                instance.name = $"Level Button - {data.levelNumber}";

                instance.PressedThis += OnLevelSelected;
                instance.SetStarsAmount(Mathf.FloorToInt(data.score * 3));
                instance.LevelNumber = data.levelNumber;
                if (data.locked)
                    instance.SetLocked();
            }
        }

        /// <summary>
        /// Clears the levels from the page.
        /// </summary>
        private void ClearLevels()
        {
            for (int i = 0; i < _levelButtonInstances.Count; i++)
            {
                LevelButton instance = _levelButtonInstances[i];
                instance.transform.SetParent(instance.transform.parent.parent);
                DTTween.Value(instance.transform.localScale.x, 0, 0.3f, 0, Easing.EASE_IN_BACK,
                    onValueChanged: (float val) => instance.transform.localScale = Vector3.one * val,
                    onComplete: () => Destroy(instance.gameObject));
            }

            _levelButtonInstances.Clear();
        }
        public bool isPressed = true;
        /// <summary>
        /// Called when a level button is selected.
        /// </summary>
        /// <param name="button">The button that was clicked on.</param>
        public void OnLevelSelected(LevelButton button)
        {
            if (GameUI.instance.isFinished)
            {
                GameUI.instance.isFinished = false;
            }
            isPressed = true;
            SelectedLevel = button;
            LevelSelected?.Invoke(_database.Data[button.LevelNumber - 1]);
            StartCoroutine(ButtonWait());
        }
        IEnumerator ButtonWait()
        {
            yield return new WaitForSeconds(1f);
            isPressed = false;
        }
    }
}