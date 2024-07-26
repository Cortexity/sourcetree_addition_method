using DTT.MinigameBase.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Handles rendering the Nonogram. Spawns in tiles and numbers to make the Nonogram visable.
    /// </summary>
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridRenderer : MonoBehaviour
    {
        public GamePopupUI _popup;

        public static GridRenderer instance;
        /// <summary>
        /// Reference to the Nonogram manager, used to check if Nonogram is complete.
        /// </summary>
        [SerializeField]
        public NonogramManager _manager;

        /// <summary>
        /// Reference to the input manager. Given to the tiles to manage input.
        /// </summary>
        [SerializeField]
        private InputManager _inputManager;

        /// <summary>
        /// Reference to the color manager. Used to spawn in buttons to select color.
        /// </summary>
        [SerializeField]
        private ColorManager _colorManager;

        /// <summary>
        /// Reference to the validator. Used to check if the Nonogram is complete.
        /// </summary>
        [SerializeField]
        private Validator _validator;

        /// <summary>
        /// Tile prefab, used for filling the grid.
        /// </summary>
        [SerializeField]
        private TileBehaviour _TilePrefab;

        /// <summary>
        /// Parent to the Nonogram grid.
        /// </summary>
        [SerializeField]
        private RectTransform _gridParent;

        /// <summary>
        /// Prefab for the numbers to the sides of the Nonogram.
        /// </summary>
        [SerializeField]
        private NumberDisplay _numberDisplayPrefab;

        /// <summary>
        /// Parent to the numbers on the vertical axes around the Nonogram.
        /// </summary>
        [SerializeField]
        private Transform _numberParentVertical;

        /// <summary>
        /// Parent to the numbers on the horizontal axes around the Nonogram.
        /// </summary>
        [SerializeField]
        private Transform _numberParentHorizontal;

        /// <summary>
        /// The parent of the grid.
        /// </summary>
        [SerializeField]
        private RectTransform _container;

        /// <summary>
        /// Scales the grid to preserve the squares tiles.
        /// </summary>
        [SerializeField]
        private GridScaler _scaler;

        /// <summary>
        /// Used for centering the Nonogram grid.
        /// </summary>
        private Vector3 _centerPos;

        /// <summary>
        /// Used for checking all the tiles and their states.
        /// </summary>
        private List<Tile> _allTiles;

        /// <summary>
        /// Stores all tiles to instantly fill the board.
        /// </summary>
        public List<TileBehaviour> _tiles;
        public List<Tile> _abc;

        /// <summary>
        /// Holds all horizontal numbers. Used to clear the numbers on the board.
        /// </summary>
        private List<NumberDisplay> _horizontalNumbers;

        /// <summary>
        /// Holds all horizontal numbers. Used to clear the numbers on the board.
        /// </summary>
        private List<NumberDisplay> _verticalNumbers;

        /// <summary>
        /// The fontsize of the numbers on the side of the Nonogram. + 1 for spacing.
        /// </summary>
        private const int _FONTSIZE_PLUS_SPACING = 31;

        public List<Color> Pos = new List<Color>();

        public List<Color> PosA = new List<Color>();

        public List<int> AllNumber = new List<int>();

        public int FindNumberGrid;

        public List<GameObject> Heart = new List<GameObject>();

        public AudioSource Audio;
        public AudioClip ClueFound;
        public AudioClip ClueWrong;
        public AudioClip LineComplete;
        public AudioClip Lose;

        private void Awake()
        {
            instance = this;
            GameUI.instance.HeartCounter = 3;
        }

        public void FullHeart()
        {
            GameUI.instance.HeartCounter = 3;
            for (int i = 0; i < Heart.Count; i++)
            {
                Heart[i].gameObject.SetActive(true);
            }
        }

        public void HeartOnOff()
        {
            for (int i = 0; i < Heart.Count; i++)
            {
                if (GameUI.instance.HeartCounter > i)
                {
                    Heart[i].gameObject.SetActive(true);
                }
                else
                {
                    Heart[i].gameObject.SetActive(false);
                }
            }
        }
        public void HeartOn()
        {
            for (int i = 0; i < Heart.Count; i++)
            {
                if (GameUI.instance.HeartCounter > i)
                {
                    Heart[i].gameObject.SetActive(true);
                }
            }
        }

        public int CheckRowAvailble;
        public int CheckCurrentRow;

        public int CheckColAvailble;
        public int CheckCurrentCol;

        public bool IsDone(int j, List<Color> temps)
        {
            if (temps[j].r == 0 && temps[j].g == 0 && temps[j].b == 0 && temps[j].a == 1)
                return true;

            return false;
        }


        public void FindCol(int col)
        {
            CheckColAvailble = 0;
            CheckCurrentCol = 0;

            Debug.Log("<color=yellow>" + col + "</color>");

            if (FindNumberGrid == 5)
            {
                ShowTileForColumn(col, 5);
            }
            else if (FindNumberGrid == 10)
            {
                ShowTileForColumn(col, 10);
            }
            else if (FindNumberGrid == 15)
            {
                ShowTileForColumn(col, 15);
            }
            else if (FindNumberGrid == 20)
            {
                ShowTileForColumn(col, 20);
            }
        }

        public void ShowTileForColumn(int col, int gridSize)
        {
            for (int i = 0; i < gridSize; i++)
            {
                int index = i * gridSize + col;
                if (IsDone(index, Pos))
                {
                    CheckColAvailble++;
                }
            }

            for (int i = 0; i < gridSize; i++)
            {
                int index = i * gridSize + col;
                if (IsDone(index, Pos) && IsDone(index, PosA))
                {
                    Debug.Log("Pos A Bool " + IsDone(index, PosA));
                    Debug.Log("Index Pos A " + index);
                    Debug.Log("Pos A " + PosA.Count);
                    CheckCurrentCol++;
                }
            }

            if (CheckCurrentCol == CheckColAvailble)
            {
                Audio.PlayOneShot(LineComplete);
                for (int i = 0; i < gridSize; i++)
                {
                    int index = i * gridSize + col;
                    if (!_tiles[index].IsComplate)
                    {
                        // Debug.Log("Not");
                        _tiles[index]._xMark.color = _markColor;
                        _tiles[index]._xMark.sprite = _xTexture;
                        _tiles[index].ChangeTileStatus(TileMark.NOT_POSSIBLE);
                    }
                    else
                    {
                        // Debug.Log("Yes");
                    }
                }
            }
        }

        public void FindRow(int row)
        {
            CheckRowAvailble = 0;
            CheckCurrentRow = 0;
            //   Debug.Log("<color=yellow>" + row + "</color>");
            if (FindNumberGrid == 5)
            {
                int start = row * 5;
                int end = start + 5;
                ShowTile(start, end);
            }
            else if (FindNumberGrid == 10)
            {
                int start = row * 10;
                int end = start + 10;
                ShowTile(start, end);
            }
            else if (FindNumberGrid == 15)
            {
                int start = row * 15;
                int end = start + 15;
                ShowTile(start, end);
            }
            else if (FindNumberGrid == 20)
            {
                int start = row * 20;
                int end = start + 20;
                ShowTile(start, end);
            }
        }

        public void ShowTile(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                //  Debug.Log(GridRenderer.instance._tiles[i].name + "::::::");

                if (IsDone(i, Pos))
                {
                    CheckRowAvailble++;
                    //   Debug.Log("ROW AVAILABLE");
                }
            }

            for (int j = start; j < end; j++)
            {
                //   Debug.Log(GridRenderer.instance._tiles[j].name + "::");

                if (IsDone(j, Pos) && IsDone(j, PosA)) 
                {
                    CheckCurrentRow++;
                    // Debug.Log("<color=yellow>YES DONE</color>");
                }
                else
                {
                    //  Debug.Log("<color=yellow>No DONE</color>");
                }
            }

            if (CheckCurrentRow == CheckRowAvailble)
            {
                Audio.PlayOneShot(LineComplete);
                Debug.Log("<color=yellow>" + "COMPLATE" + "</color>");
                for (int j = start; j < end; j++)
                {
                    //  Debug.Log(GridRenderer.instance._tiles[j].name + "::");
                    if (!_tiles[j].IsComplate)
                    {
                        _tiles[j]._xMark.color = _markColor;
                        _tiles[j]._xMark.sprite = _xTexture;
                        _tiles[j].ChangeTileStatus(TileMark.NOT_POSSIBLE);
                        _tiles[j].GetComponent<TileBehaviour>().IsComplate = true;
                        Debug.Log("AA");
                    }
                    else
                    {
                        // Debug.Log("BB");
                    }
                }
            }
        }

        public void FinishGameOver()
        {
            Debug.Log("Finish");
            Audio.PlayOneShot(Lose);
            _popup.gameObject.SetActive(true);
            _popup.Show(true);
            _popup.SetTitleToFinishedGameOver();
            _popup.EnableNextLevelButton(false);
        }

        [SerializeField]
        public Sprite _xTexture;
        [SerializeField]
        public Color _markColor = Color.black;

        public int TileSize = 50;

        /// <summary>
        /// Sets the RenderGrid to be called when done generating the Nonogram.
        /// </summary>
        private void OnEnable()
        {
            _manager.GridHandler.NonogramDataGenerated += RenderGrid;

            _tiles = new List<TileBehaviour>();
            _allTiles = new List<Tile>();
            _horizontalNumbers = new List<NumberDisplay>();
            _verticalNumbers = new List<NumberDisplay>();
            _centerPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }

        /// <summary>
        /// Removes the event to render the Nonogram.
        /// </summary>
        private void OnDisable() => _manager.GridHandler.NonogramDataGenerated -= RenderGrid;

        /// <summary>
        /// Handles the size of the tiles inside the grid and fills in the grid with tiles and numbers.
        /// </summary>
        /// <param name="data">The Nonogram data needed to generate the grid.</param>
        public void RenderGrid(NonogramData data)
        {
            _container.SetPositionAndRotation(_centerPos, Quaternion.identity);

            foreach (TileBehaviour item in _tiles)
                Destroy(item.gameObject);
            foreach (NumberDisplay item in _verticalNumbers)
                Destroy(item.gameObject);
            foreach (NumberDisplay item in _horizontalNumbers)
                Destroy(item.gameObject);

            _allTiles.Clear();
            _tiles.Clear();
            _verticalNumbers.Clear();
            _horizontalNumbers.Clear();
            Pos.Clear();
            PosA.Clear();

            Color[] grid = data.grid;
            int width = data.gridSize.x;
            int height = data.gridSize.y;

            int totalSize = width * height;

            int i, j;

            int counter = 0;

            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {

                    SpawnTile(grid[counter], i, j, counter);
                    counter++;
                }
            }

            if (_tiles.Count == 25)
            {
                FindNumberGrid = 5;
            }
            else if (_tiles.Count == 100)
            {
                FindNumberGrid = 10;

            }
            else if (_tiles.Count == 225)
            {
                FindNumberGrid = 15;
            }
            else if (_tiles.Count == 400)
            {
                FindNumberGrid = 20;
            }

            //for (int x = 0; x < totalSize; x++)
            //{
            //    SpawnTile(grid[x], 1,1);
            //}

            //for (int i = 0; i < width; i++)
            //{
            //    for (int j = 0; j < height; j++)
            //    {
            //        if (grid[i*j] == new Color(1,1,1,1))
            //        {
            //            Debug.Log("AA");
            //         //   Pos.Add(grid[i * j]);
            //            AllNumber.Add(i);
            //        }
            //        else
            //        {
            //            Debug.Log("BB");
            //          //  Pos.Add(grid[i * j]);
            //            AllNumber.Add(i);
            //        }

            //    }
            //}

            _validator.SetTiles(_allTiles, _tiles);

            List<int> allNumbersLengths = new List<int>();

            int columnLength = data.rows.Length;
            for (int rowId = 0; rowId < columnLength; rowId++)
            {
                if (data.rows[rowId].numbers.Length > 0)
                {
                    allNumbersLengths.Add(data.rows[rowId].numbers.Length);
                    NumberDisplay newNumberDisplay = Instantiate(_numberDisplayPrefab, _numberParentHorizontal);
                    newNumberDisplay.RenderNumbers(data.rows[rowId].numbers, data.rows[rowId].numberColor, true, data.type);
                    _horizontalNumbers.Add(newNumberDisplay);
                    Debug.Log("Rows Length " + data.rows.Length);
                }
            }

            int rowLength = data.columns.Length - 1;
            for (int columnId = rowLength; columnId > -1; columnId--)
            {
                if (data.columns[columnId].numbers.Length > 0)
                {
                    allNumbersLengths.Add(data.columns[columnId].numbers.Length);
                    NumberDisplay newNumberDisplay = Instantiate(_numberDisplayPrefab, _numberParentVertical);
                    newNumberDisplay.RenderNumbers(data.columns[columnId].numbers, data.columns[columnId].numberColor, false, data.type);
                    _verticalNumbers.Add(newNumberDisplay);
                }
            }

            if (FindNumberGrid == 5)
            {
                TileSize = 140;
            }
            else if (FindNumberGrid == 10)
            {
                TileSize = 60;
            }
            else if (FindNumberGrid == 15)
            {
                TileSize = 40;
            }
            else if (FindNumberGrid == 20)
            {
                TileSize = 25;
            }

            int numberGroupLength = allNumbersLengths.Max() * _FONTSIZE_PLUS_SPACING;
            _scaler.ScaleGrid(new Vector2Int(width, height), numberGroupLength, TileSize);
            _colorManager.SpawnColorButtons(data.differentColors);
        }

        /// <summary>
        /// Spawns in a grid tile of the Nonogram and sets the correct states of the tiles.
        /// </summary>
        /// <param name="correctColor">The color to set as correct.</param>
        
        public void SpawnTile(Color correctColor, int row, int col, int n)
        {
            Tile newTile = new Tile();
            newTile.SetCorrectStatus(correctColor);
            TileBehaviour newTileBehaviour = Instantiate(_TilePrefab, _gridParent);

            newTileBehaviour.number = n;
            newTileBehaviour.row = row;
            newTileBehaviour.col = col;
            newTileBehaviour.name = row + " :: " + col;

            newTileBehaviour.Initialize(newTile, _manager, _inputManager, _manager.LastConfig.ColorSettings.DefaultColor);  // Level Generate....................
            _tiles.Add(newTileBehaviour);
            //  _abc.Add(newTile);
            _allTiles.Add(newTile);

            Debug.Log($"Spawned Tile at position: {newTileBehaviour.transform.position}, Total Tiles: {_tiles.Count}");

            if (newTileBehaviour.Tile.CorrectStatus != newTileBehaviour.Tile.CurrentStatus && newTileBehaviour.Tile.CurrentMark != TileMark.WRONG) 
            {
                //  newTileBehaviour.ChangeTileStatus(TileMark.WRONG);
                Pos.Add(newTileBehaviour.Tile.CorrectStatus);
                //   StartCoroutine(FirstTimeHintProvider(newTileBehaviour));
            }
            else
            {
                // newTileBehaviour.ChangeTileStatus(TileMark.NOT_POSSIBLE);
                Pos.Add(newTileBehaviour.Tile.CorrectStatus);
            }

            PosA.Add(new Color(0.5f, 0.5f, 0.5f, 1));

            Debug.Log(PosA + "::");

            //if (grid[i * j] == new Color(1, 1, 1, 1))
            //{
            //    Debug.Log("AA");
            //    //   Pos.Add(grid[i * j]);
            //    AllNumber.Add(i);
            //}
            //else
            //{
            //    Debug.Log("BB");
            //    //  Pos.Add(grid[i * j]);
            //    AllNumber.Add(i);
            //}
        }

        IEnumerator FirstTimeHintProvider(TileBehaviour tl)
        {
            tl.ChangeTileStatus(TileMark.EMPTY);
            tl.Tile.ChangeStatus(tl._defaultColor);
            yield return new WaitForSeconds(2f);
            tl.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
