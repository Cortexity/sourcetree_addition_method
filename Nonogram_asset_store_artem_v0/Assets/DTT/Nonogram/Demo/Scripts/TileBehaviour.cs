using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Makes the tiles touchable/clickable and changes the tiles states.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class TileBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
    {
        /// <summary>
        /// The letter used for the wrong mark.
        /// </summary>
        [SerializeField]
        private Image _xMark;

        /// <summary>
        /// The texture used for the Xmark.
        /// </summary>
        [SerializeField]
        private Sprite _xTexture;

        /// <summary>
        /// The texture used for the hint mark.
        /// </summary>
        [SerializeField]
        private Sprite _pencilTexture;

        /// <summary>
        /// The color the tiles toggle to when using toggle input.
        /// </summary>
        [SerializeField]
        private Color _markColor = Color.magenta;

        /// <summary>
        /// The color the tiles toggle to when using toggle input.
        /// </summary>
        [SerializeField]
        private Color _hintMarkColor = Color.cyan;

        /// <summary>
        /// The color the tiles will be at the start of the game.
        /// </summary>
        private Color _defaultColor;

        /// <summary>
        /// Reference to the manager.
        /// </summary>
        private NonogramManager _manager;

        /// <summary>
        /// Reference to the color manager.
        /// </summary>
        private InputManager _inputManager;

        /// <summary>
        /// Stores the image attached to this gameobject.
        /// </summary>
        private Image _imageComponent;

        /// <summary>
        /// The color the tile was when marked by a hint. Used to make the mark reappear when the tile changes to that color.
        /// </summary>
        private Color _wrongedColor;

        /// <summary>
        /// Tile info held by the tile you can click, granted by gridRenderer.
        /// </summary>
        public Tile Tile { get; private set; }

        /// <summary>
        /// Sets the tile and sets size of the hitbox.
        /// </summary>
        /// <param name="tile">The Tile that holds the data of this grid piece</param>
        /// <param name="manager">The Nonogram manager.</param>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="defaultColor">The Color used as default for the tiles.</param>
        public void Initialize(Tile tile, NonogramManager manager, InputManager inputManager, Color defaultColor)
        {
            Tile = tile;
            _imageComponent = GetComponent<Image>();

            _manager = manager;
            _inputManager = inputManager;
            _defaultColor = defaultColor;

            Tile.Initialize(_manager.LastConfig.ColorSettings.DefaultColor, _manager.LastConfig.ColorSettings.ToggleColor);
            ChangeTileStatus(_defaultColor);
        }

        /// <summary>
        /// Changes the tile state if no mark selected.
        /// </summary>
        /// <param name="mark">The new mark to set.</param>
        public void ChangeTileStatus(TileMark mark) => ChangeTileStatus(Color.white, mark);

        /// <summary>
        /// Changes the tile state if no mark selected.
        /// </summary>
        /// <param name="newColor">The new color to set.</param>
        public void ChangeTileStatus(Color newColor) => ChangeTileStatus(newColor, TileMark.EMPTY);

        /// <summary>
        /// Changes the tile state if no mark selected.
        /// </summary>
        /// <param name="newColor">The new color to set.</param>
        /// <param name="mark">The new mark to set.</param>
        public void ChangeTileStatus(Color newColor, TileMark mark)
        {
            switch (mark)   
            {
                case TileMark.EMPTY:
                    Tile.ChangeStatus(newColor);
                    if (Tile.CurrentStatus == _wrongedColor)
                        ChangeTileStatus(TileMark.WRONG);
                    break;
                case TileMark.NOT_POSSIBLE:
                    _xMark.color = _markColor;
                    _xMark.sprite = _xTexture;
                    Tile.ChangeMark(mark);
                    break;
                case TileMark.WRONG:
                    _xMark.color = _hintMarkColor;
                    _xMark.sprite = _pencilTexture;
                    _wrongedColor = Tile.CurrentStatus;
                    Tile.ChangeMark(mark);
                    break;
            }

            SetTileImage();
        }

        /// <summary>
        /// Toggles the tile between black/white.
        /// </summary>
        /// <param name="mark">The new mark to set.</param>
        public void ToggleTileStatus(TileMark mark) => ToggleTileStatus(false, mark);

        /// <summary>
        /// Toggles the tile between black/white.
        /// </summary>
        /// <param name="newStatus">The new status to set.</param>
        public void ToggleTileStatus(bool newStatus) => ToggleTileStatus(newStatus, TileMark.EMPTY);

        /// <summary>
        /// Toggles the tile between black/white.
        /// </summary>
        /// <param name="newStatus">The new status to set.</param>
        /// <param name="mark">The new mark to set.</param>
        public void ToggleTileStatus(bool newStatus, TileMark mark)
        {
            switch (mark)
            {
                case TileMark.EMPTY:
                    Tile.ToggleStatus(newStatus);
                    if (Tile.CurrentStatus == _wrongedColor)
                        ToggleTileStatus(TileMark.WRONG);
                    break;
                case TileMark.NOT_POSSIBLE:
                    _xMark.color = _markColor;
                    _xMark.sprite = _xTexture;
                    Tile.ChangeMark(mark);
                    break;
                case TileMark.WRONG:
                    _xMark.color = _hintMarkColor;
                    _xMark.sprite = _pencilTexture;
                    _wrongedColor = Tile.CurrentStatus;
                    Tile.ChangeMark(mark);
                    break;
            }

            SetTileImage();
        }

        /// <summary>
        /// Sets the color of a tile based on current tile state.
        /// </summary>
        public void SetTileImage()
        {
            _imageComponent.color = Tile.CurrentStatus;

            if(Tile.CurrentMark == TileMark.NOT_POSSIBLE || Tile.CurrentMark == TileMark.WRONG)
                _xMark.gameObject.SetActive(true);
            else
                _xMark.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets held tile to the correct value.
        /// </summary>
        public void SetTileToCorrect()
        {
            Tile.ChangeStatus(Tile.CorrectStatus);
            Tile.ChangeMark(TileMark.EMPTY);
            SetTileImage();
        }

        /// <summary>
        /// Changes states of tiles touched with drag after the first.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_inputManager.UseDrag)
                return;

            if (_inputManager.StartedDrag)
            {
                switch (_manager.InputType)
                {
                    case SelectionType.DEFAULT_TOGGLE:
                        ToggleTileStatus(_inputManager.LastSelectedState, _inputManager.SelectedMark);
                        break;
                    case SelectionType.SELECT_COLOR:
                        ChangeTileStatus(_inputManager.CurrentSelectedColor, _inputManager.SelectedMark);
                        break;
                }
            }
        }

        /// <summary>
        /// Ends drag.
        /// </summary>
        public void OnPointerUp(PointerEventData eventData) => _inputManager.SetStartDrag(false);

        /// <summary>
        /// Sets the tileState when the drag starts.
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_inputManager.UseDrag)
                return;

            switch (_manager.InputType)
            {
                case SelectionType.DEFAULT_TOGGLE:
                    _inputManager.SetDragStartState(Tile.CurrentStatus == _defaultColor);
                    ToggleTileStatus(_inputManager.LastSelectedState, _inputManager.SelectedMark);
                    break;
                case SelectionType.SELECT_COLOR:
                    ChangeTileStatus(_inputManager.CurrentSelectedColor, _inputManager.SelectedMark);
                    break;
            }

            _inputManager.SetStartDrag(true);
        }
    }
}