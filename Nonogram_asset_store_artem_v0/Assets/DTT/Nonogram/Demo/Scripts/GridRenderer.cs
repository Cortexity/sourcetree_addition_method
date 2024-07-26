using System.Collections.Generic;
using UnityEngine;
using DTT.MinigameBase.Timer;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Handles rendering the Nonogram. Spawns in tiles and numbers to make the Nonogram visable.
    /// </summary>
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridRenderer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Nonogram manager, used to check if Nonogram is complete.
        /// </summary>
        [SerializeField] 
        private NonogramManager _manager;

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
        private List<TileBehaviour> _tiles;

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
            // Sets the grid to the center of the screen
            _container.SetPositionAndRotation(_centerPos, Quaternion.identity);

            // Removes the old grid
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

            // Gets info needed to render the grid
            Color[] grid = data.grid;
            int width = data.gridSize.x;
            int height = data.gridSize.y;

            // Renders a new grid with tiles and numbers
            int totalSize = width * height;
            for (int x = 0; x < totalSize; x++)
                SpawnTile(grid[x]);

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

            int numberGroupLength = allNumbersLengths.Max() * _FONTSIZE_PLUS_SPACING;

            _scaler.ScaleGrid(new Vector2Int(width, height), numberGroupLength);
            _colorManager.SpawnColorButtons(data.differentColors);
        }

        /// <summary>
        /// Spawns in a grid tile of the Nonogram and sets the correct states of the tiles.
        /// </summary>
        /// <param name="correctColor">The color to set as correct.</param>
        private void SpawnTile(Color correctColor)
        {
            Tile newTile = new Tile();
            newTile.SetCorrectStatus(correctColor);

            TileBehaviour newTileBehaviour = Instantiate(_TilePrefab, _gridParent);
            newTileBehaviour.Initialize(newTile, _manager, _inputManager, _manager.LastConfig.ColorSettings.DefaultColor);

            _tiles.Add(newTileBehaviour);
            _allTiles.Add(newTile);
        }
    }
}
