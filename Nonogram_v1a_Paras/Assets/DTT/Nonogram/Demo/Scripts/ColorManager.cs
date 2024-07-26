using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Handles the color buttons.
    /// </summary>
    public class ColorManager : MonoBehaviour
    {
        /// <summary>
        /// The outline color of the selected button.
        /// </summary>
        [SerializeField]
        private Color _selectedButtonColor = Color.green;

        /// <summary>
        /// Prefab used for the color buttons.
        /// </summary>
        [SerializeField]
        private Button _colorButtonPrefab;

        /// <summary>
        /// Prefab used for the x-mark button.
        /// </summary>
        [SerializeField]
        private Button _xMarkButton;

        /// <summary>
        /// Prefab used to clear the current mark.
        /// </summary>
        [SerializeField]
        private Button _clearButton;

        /// <summary>
        /// The parent to the spawned in buttons.
        /// </summary>
        [SerializeField]
        private Transform _buttonParent;

        /// <summary>
        /// Reference to the input manager. Handles the button input.
        /// </summary>
        [SerializeField]
        private InputManager _inputManager;

        /// <summary>
		/// Reference to the Nonogram manager. Sets SetMenu when Nonogram data is generated.
		/// </summary>
        [SerializeField]
        private NonogramManager _manager;

        /// <summary>
        /// The current inputType.
        /// </summary>
        private SelectionType _currentGamemode;

        /// <summary>
        /// Holds all the color buttons, used to turn off the current selection.
        /// </summary>
        private List<Image> allColorButtons;

        /// <summary>
        /// Sets onClick listeners of mark and clear button. Attaches SetMenu to GeneratedNonogramData.
        /// </summary>
        private void OnEnable() => _manager.GridHandler.NonogramDataGenerated += SetMenu;

        /// <summary>
        /// removes the listeners.
        /// </summary>
        private void OnDisable() => _manager.GridHandler.NonogramDataGenerated -= SetMenu;

        /// <summary>
        /// Sets selection to the mark: not possible.
        /// </summary>
        private void SetMarkButton() => _inputManager.SetNewSelection(Color.white, TileMark.NOT_POSSIBLE);

        /// <summary>
        /// Turns on/off clear button depending on inputType of the loaded config.
        /// </summary>
        /// <param name="data">The generated Nonogram data.</param>
        private void SetMenu(NonogramData data) => _currentGamemode = data.type;

        /// <summary>
        /// Spawns in a button for each color in the given array.
        /// </summary>
        /// <param name="allColors">Colors to spawn buttons for.</param>
        public void SpawnColorButtons(IEnumerable<Color> allColors)
        {
            foreach (Transform item in _buttonParent)
                Destroy(item.gameObject);

            allColorButtons = new List<Image>();

            Button xmark = Instantiate(_xMarkButton, _buttonParent);
            XMarkSetup(xmark);

            if (_currentGamemode == SelectionType.DEFAULT_TOGGLE)
            {
                Button clearButton = Instantiate(_clearButton, _buttonParent);
                allColorButtons.Add(clearButton.GetComponent<Image>());
                ClearButtonSetup(clearButton, _manager.LastConfig.ColorSettings.DefaultColor);
                SetSelectedColor(clearButton);
                _inputManager.SetNewSelection(_manager.LastConfig.ColorSettings.DefaultColor);
            }
            if (_currentGamemode == SelectionType.SELECT_COLOR)
            {
                _buttonParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (allColors.Count() + 2) * 110);

                Button defaultColorButton = Instantiate(_colorButtonPrefab, _buttonParent);
                ButtonSetup(defaultColorButton, _manager.LastConfig.ColorSettings.DefaultColor);

                bool firstButton = true;
                foreach (Color color in allColors)
                {
                    Button newButton = Instantiate(_colorButtonPrefab, _buttonParent);
                    ButtonSetup(newButton, color);

                    if (firstButton)
                    {
                        firstButton = false;
                        SetSelectedColor(newButton);
                    }
                }

                // Sets the starting selection to be the first color.
                Color firstSelection = Color.white;
                IEnumerator<Color> en = allColors.GetEnumerator();
                if (en.MoveNext())
                    firstSelection = en.Current;
                _inputManager.SetNewSelection(firstSelection);
            }
        }

        /// <summary>
        /// Sets the given color to button and applies this color to current selected color when clicked.
        /// </summary>
        /// <param name="button">The button to apply the settings on.</param>
        /// <param name="color">The color of the button and new selection</param>
        private void ButtonSetup(Button button, Color color)
        {
            allColorButtons.Add(button.GetComponent<Image>());
            button.GetComponent<ColorButton>().ChangeColor(color);
            button.onClick.AddListener(() => _inputManager.SetNewSelection(color));
            button.onClick.AddListener(UnSelectButtons);
            button.onClick.AddListener(() => SetSelectedColor(button));
        }

        /// <summary>
        /// Sets up the X-mark button.
        /// </summary>
        /// <param name="button">The button to apply the settings on.</param>
        private void XMarkSetup(Button button)
        {
            allColorButtons.Add(button.GetComponent<Image>());
            button.onClick.AddListener(SetMarkButton);
            button.onClick.AddListener(UnSelectButtons);
            button.onClick.AddListener(() => SetSelectedColor(button));
        }

        /// <summary>
        /// Sets up the clear button for toggle input.
        /// </summary>
        /// <param name="button">The button to apply the settings on.</param>
        /// <param name="color">The default color used in the Nonogram.</param>
        private void ClearButtonSetup(Button button, Color color)
        {
            allColorButtons.Add(button.GetComponent<Image>());
            button.onClick.AddListener(() => _inputManager.SetNewSelection(color));
            button.onClick.AddListener(UnSelectButtons);
            button.onClick.AddListener(() => SetSelectedColor(button));
        }

        /// <summary>
        /// Sets teh outline color of the buttons back to black.
        /// </summary>
        private void UnSelectButtons()
        {
            foreach (Image button in allColorButtons)
                button.color = Color.black;
        }

        /// <summary>
        /// Sets the selected button outline to green.
        /// </summary>
        /// <param name="button">The button of which the color will be changed.</param>
        private void SetSelectedColor(Button button) => button.GetComponent<Image>().color = _selectedButtonColor;
    }
}