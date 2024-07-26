using DTT.MinigameBase.LevelSelect;
using DTT.MinigameBase.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Makes the tiles touchable/clickable and changes the tiles states.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class TileBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
    {
        public static TileBehaviour instance;
        public int row, col;

        public int number;

        public bool IsComplate = false;
        /// <summary>
        /// The letter used for the wrong mark.
        /// </summary>
        [SerializeField]
        public Image _xMark;

        /// <summary>
        /// The texture used for the Xmark.
        /// </summary>
        [SerializeField]
        public Sprite _xTexture;

        /// <summary>
        /// The texture used for the hint mark.
        /// </summary>
        [SerializeField]
        public Sprite _pencilTexture;

        /// <summary>
        /// The color the tiles toggle to when using toggle input.
        /// </summary>
        [SerializeField]
        public Color _markColor = Color.black;

        /// <summary>
        /// The color the tiles toggle to when using toggle input.
        /// </summary>
        [SerializeField]
        private Color _hintMarkColor = Color.cyan;

        /// <summary>
        /// The color the tiles will be at the start of the game.
        /// </summary>
        public Color _defaultColor;

        /// <summary>
        /// Reference to the manager.
        /// </summary>

        [SerializeField]
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
        private void Awake()
        {
            instance = this;
        }
        /// <summary>
        /// Sets the tile and sets size of the hitbox.
        /// </summary>
        /// <param name="tile">The Tile that holds the data of this grid piece</param>
        /// <param name="manager">The Nonogram manager.</param>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="defaultColor">The Color used as default for the tiles.</param>
        public void Initialize(Tile tile, NonogramManager manager, InputManager inputManager, Color defaultColor)
        {
            // Debug.Log("Enter in Tile Script");
            //_manager.HintUsed += Validator.instance.GiveHint;
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
            //   Debug.Log("mark " + mark);
            //   Debug.Log("Tile " + Tile.CurrentStatus);
            switch (mark)
            {
                case TileMark.EMPTY:
                    GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueFound);
                    Tile.ToggleStatus(newStatus);
                    if (Tile.CurrentStatus == _wrongedColor)
                        ToggleTileStatus(TileMark.WRONG);
                    break;
                case TileMark.NOT_POSSIBLE:
                    if (isDone)
                    {
                        _xMark.color = _markColor;
                        _xMark.sprite = _xTexture;
                        Tile.ChangeMark(mark);
                    }
                    else
                    {
                        GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueWrong);
                        Tile.ToggleStatus(newStatus);
                        if (Tile.CurrentStatus == _wrongedColor)
                            ToggleTileStatus(TileMark.WRONG);
                        GameUI.instance.HeartCounter--;
                        GridRenderer.instance.HeartOnOff();

                        if (GameUI.instance.HeartCounter == 0)
                        {
                            GridRenderer.instance.FinishGameOver();
                            return;
                        }
                    }
                    break;
                case TileMark.WRONG:
                    _xMark.color = _hintMarkColor;
                    _xMark.sprite = _pencilTexture;
                    _wrongedColor = Tile.CurrentStatus;
                    Tile.ChangeMark(mark);
                    break;
            }
            Validator.instance.ValidateNonogram();
            SetTileImage();
        }

        /// <summary>
        /// Sets the color of a tile based on current tile state.
        /// </summary>
        private void Update()
        {
            if (GameUI.instance.isFinished)
            {
                _xMark.color = Color.clear;
            }
        }

        public void SetTileImage()
        {



            /* if (GameUI.instance.isFinished)
             {
                 if (Tile.CurrentStatus == Color.black)
                 {
                     Debug.Log("REDDDDDDDDDDDDDDDDDDD");
                     _imageComponent.color = UnityEngine.Random.ColorHSV();
                     _xMark.gameObject.SetActive(false);
                     _xMark.sprite = null;
                 }
                 else
                 {
                     Debug.Log("CURREENNNTTTTTTTTTTTT");
                     _imageComponent.color = Tile.CurrentStatus;
                 }
             }
             else
             {
                 _imageComponent.color = Tile.CurrentStatus;
             }
             if (Tile.CurrentMark == TileMark.NOT_POSSIBLE || Tile.CurrentMark == TileMark.WRONG)
                 _xMark.gameObject.SetActive(true);
             else
                 _xMark.gameObject.SetActive(false);*/

            StartCoroutine(FinishAnim());
        }
        IEnumerator FinishAnim()
        {
            float duration = 0.07f;
            float delay = duration / (GridRenderer.instance._tiles.Count - 1);

            if (GameUI.instance.isFinished)
            {
                if (Tile.CurrentStatus == Color.black)
                {
                    Debug.Log("<color=black>" + "777F  1" + "</color>");
                    _imageComponent.color = UnityEngine.Random.ColorHSV();
                    _xMark.gameObject.SetActive(false);
                    _xMark.sprite = null;

                    for (int i = GridRenderer.instance._tiles.Count - 1; i >= 0; i--)
                    {

                        if (IsDone(i, GridRenderer.instance.Pos, 0f, 1f) == IsDone(i, GridRenderer.instance.PosA, 0f, 1f))
                        {
                            bool startScaling = false;
                            GameObject tileObject = GridRenderer.instance._tiles[i].gameObject;
                            tileObject.SetActive(true);
                            if (i < GridRenderer.instance._tiles.Count - 1)
                            {
                                tileObject.transform.localScale = Vector3.zero;
                            }

                            if (!startScaling && tileObject.GetComponent<TileBehaviour>().Tile.CurrentStatus == Color.black)
                            {
                                startScaling = true;
                            }

                            if (startScaling)
                            {
                                float t = 0;
                                while (t < duration)
                                {
                                    t += Time.deltaTime;
                                    float scale = Mathf.Lerp(0, 1, t / duration);
                                    tileObject.transform.localScale = new Vector3(scale, scale, 1);
                                    yield return null;
                                }
                            }

                            tileObject.transform.localScale = Vector3.one;

                            if (i > 0 && startScaling)
                            {
                                yield return new WaitForSeconds(delay);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("<color=black>" + "777F  2" + "</color>");
                    _imageComponent.color = Tile.CurrentStatus;
                }
            }
            else
            {
                _imageComponent.color = Tile.CurrentStatus;
            }

            if (Tile.CurrentMark == TileMark.NOT_POSSIBLE || Tile.CurrentMark == TileMark.WRONG)
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
        public bool isDone = false;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!LevelSelect.instance.isPressed && !GameUI.instance.isPop)
            {
                if (IsComplate)
                    return;

                IsComplate = true;

                if (IsDone(number, GridRenderer.instance.Pos, 0f, 1f) == IsDone(number, GridRenderer.instance.PosA, 0.5f, 1f))
                {
                    isDone = false;
                    GridRenderer.instance.PosA[number] = new Color(0, 0, 0, 1);
                    Debug.Log("Pos A Number " + number);

                    /* if (_xMark.sprite == _xTexture)
                     {
                         Debug.Log("Empty Texture >>>>>>>>>>>>>>>>>>>>>>>>>");
                         Tile.ChangeMark(TileMark.EMPTY);
                         _xMark.gameObject.SetActive(false);
                     }*/
                }
                else
                {
                    isDone = true;
                    if (GameUI.instance.HeartCounter <= 3)
                    {
                        GameUI.instance.HeartCounter--;
                        GridRenderer.instance.HeartOnOff();

                        if (GameUI.instance.HeartCounter == 0)
                        {
                            GridRenderer.instance.FinishGameOver();
                            return;
                        }
                        else
                        {
                            IsClose = true;
                            if (InputManager.Instance.SelectedMark == TileMark.EMPTY)
                            {
                                GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueWrong);
                                ChangeTileStatus(TileMark.NOT_POSSIBLE);
                            }
                            else
                            {
                                GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueFound);
                                ChangeTileStatus(TileMark.NOT_POSSIBLE);
                                GameUI.instance.HeartCounter++;
                                GridRenderer.instance.HeartOn();
                            }
                        }
                    } 
                }

                // Assuming row is 0-based index
                int rowIndexA = row;  // Convert row to 0-based index if necessary

                /*   int mycounterA = 0;
                   int mycounterB = 0;

                   int filledCount = 0;
                   int totalElementsInRow = 0;*/

                int numColumns = 5;
                int startIndex = rowIndexA * numColumns;
                int endIndex = startIndex + numColumns;

                bool allFilled = true;

                // Check if all elements in the row are filled with Color(0, 0, 0, 1)
                for (int i = startIndex; i < endIndex; i++)
                {
                    Color color = GridRenderer.instance.Pos[i];
                    //   Debug.Log($"Checking element at index {i}, color: {color}");

                    if (color != new Color(0f, 0f, 0f, 1f))
                    {
                        allFilled = false;
                        break;
                    }
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    Color color = GridRenderer.instance.Pos[i];
                    Debug.Log($"Final check: Element at index {i}, color: {color}");
                }

                if (_inputManager.UseDrag)
                    return;

                int rowIndex = transform.GetSiblingIndex();
                int columnIndex = transform.parent.GetSiblingIndex();

                Debug.Log($"Clicked Tile at Row Index: {rowIndex}");
                Debug.Log($"Clicked Tile at Column Index: {columnIndex}");

                switch (_manager.InputType)
                {
                    case SelectionType.DEFAULT_TOGGLE:
                        _inputManager.SetDragStartState(Tile.CurrentStatus == _defaultColor);

                        if (!IsClose)
                            ToggleTileStatus(_inputManager.LastSelectedState, _inputManager.SelectedMark);
                        break;
                    case SelectionType.SELECT_COLOR:
                        ChangeTileStatus(_inputManager.CurrentSelectedColor, _inputManager.SelectedMark);
                        break;
                }
                _inputManager.SetStartDrag(true);
            }
        }

        /// <summary>
        /// Ends drag.
        /// </summary>

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!LevelSelect.instance.isPressed && !GameUI.instance.isPop)
            {
                _inputManager.SetStartDrag(false);
                GridRenderer.instance.FindRow(row);
                GridRenderer.instance.FindCol(col);
            }
        }

        /// <summary>
        /// Sets the tileState when the drag starts.
        /// </summary>

        public bool IsClose;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsComplate)
                return;

            IsComplate = true;

            if (!LevelSelect.instance.isPressed && !GameUI.instance.isPop)
            {
                if (_inputManager.UseDrag)
                    return;

                switch (_manager.InputType)
                {
                    case SelectionType.DEFAULT_TOGGLE:
                        /* _inputManager.SetDragStartState(Tile.CurrentStatus == _defaultColor);
                         if (!IsClose)*/
                        ToggleTileStatus(_inputManager.LastSelectedState, _inputManager.SelectedMark);

                        break;
                    case SelectionType.SELECT_COLOR:
                        ChangeTileStatus(_inputManager.CurrentSelectedColor, _inputManager.SelectedMark);
                        break;
                }
            }

            /* if (allFilled)
             {
                 GridRenderer.instance.PosA[rowIndexA] = new Color(1, 1, 1, 1); // Marking with a cross (RGBA(1.000, 1.000, 1.000, 1.000))
                 Debug.Log($"Marked row {rowIndexA} with a cross in PosA.");
             }
             else
             {
                 GridRenderer.instance.PosA[rowIndexA] = new Color(0, 0, 0, 0); // No cross (or clear the mark)
                 Debug.Log($"Row {rowIndexA} does not meet the criteria for a cross in PosA.");
             }*/

            //Debug.Log(GridRenderer.instance.AllNumber.Count);


            //    if (GridRenderer.instance.AllNumber.Contains(number))
            //    {
            //        Debug.Log("Final Number is " + number);
            //        if (GridRenderer.instance.AllNumber.Count == 9)
            //        {
            //                for (int j = 0; j < number; j++)
            //                {
            //                Debug.Log("AAA " + GridRenderer.instance.Pos[j]);
            //                }
            //        }
            //    } 
        }

        public bool IsDone(int j, List<Color> temps, float s, float e)
        {
            if (temps[j].r == s && temps[j].g == s && temps[j].b == s && temps[j].a == e)
                return true;

            return false;
        }
    }
}
