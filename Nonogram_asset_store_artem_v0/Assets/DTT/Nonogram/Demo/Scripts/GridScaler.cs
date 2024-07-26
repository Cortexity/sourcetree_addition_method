using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// This class is used to scale the Nonogram based on the total grid size.
    /// </summary>
    public class GridScaler : MonoBehaviour
    {
        /// <summary>
        /// The minimum size a tile can be in both dimensions.
        /// </summary>
        [SerializeField]
        private float _minimumTileSize = 50;

        /// <summary>
        /// The parent to all the tiles.
        /// </summary>
        [SerializeField]
        private RectTransform _gridArea;

        /// <summary>
        /// The number parent on the top side.
        /// </summary>
        [SerializeField]
        private RectTransform _numberContainerTop;

        /// <summary>
        /// The number parent on the top side.
        /// </summary>
        [SerializeField]
        private RectTransform _numberContainerLeft;

        /// <summary>
        /// The background behind the grid.
        /// </summary>
        [SerializeField]
        private RectTransform _background;

        /// <summary>
        /// The spacing between the tiles on the grid.
        /// </summary>
        private const float _TILE_SPACING = 5;

        /// <summary>
        /// The offset range between the grid and the number parents.
        /// </summary>
        private const float _NUMBEROFFSET = 10;

        /// <summary>
        /// The number 2! Used for getting halve the size of the numbercontainers.
        /// </summary>
        private const int _TWO = 2;

        /// <summary>
        /// Used to calculated the center for the Nonogram.
        /// </summary>
        private const float _GRID_OFFSET_MULTIPLIER = .535f;

        /// <summary>
        /// Sets the grid sizes and positions. Making it scale with the amount of tiles needed.
        /// </summary>
        /// <param name="gridSize">The size of the Nonogram.</param>
        /// <param name="numberContainerSize">The size of the numbers containers on the sides.</param>
        public void ScaleGrid(Vector2Int gridSize, int numberContainerSize)
        {
            GridLayoutGroup layoutGroup = _gridArea.GetComponent<GridLayoutGroup>();
            layoutGroup.cellSize = new Vector2(_minimumTileSize, _minimumTileSize);
            layoutGroup.spacing = new Vector2(_TILE_SPACING, _TILE_SPACING);

            float containerSizeX = gridSize.x * _minimumTileSize + gridSize.x * _TILE_SPACING;
            float containerSizeY = gridSize.y * _minimumTileSize + gridSize.y * _TILE_SPACING;

            // Sets the size of grid
            _gridArea.sizeDelta = new Vector2(containerSizeX, containerSizeY);

            // Sets the size of the top numbers
            _numberContainerTop.sizeDelta = new Vector2(containerSizeX, numberContainerSize);
            foreach (RectTransform item in _numberContainerTop)
                item.sizeDelta = new Vector2(containerSizeX / gridSize.x - _TILE_SPACING, numberContainerSize);

            // Sets the size of the left numbers
            _numberContainerLeft.sizeDelta = new Vector2(numberContainerSize, containerSizeY );
            foreach (RectTransform item in _numberContainerLeft)
                item.sizeDelta = new Vector2(numberContainerSize, containerSizeY / gridSize.y - _TILE_SPACING);

            // Sets the size of the grid container.
            _background.sizeDelta = _gridArea.sizeDelta +
                new Vector2(_numberContainerLeft.sizeDelta.x + _NUMBEROFFSET * _TWO,
                _numberContainerTop.sizeDelta.y + _NUMBEROFFSET * _TWO);

            // Sets the position of the grid and background.
            _background.position = transform.parent.position;
            _gridArea.localPosition = new Vector3(numberContainerSize * _GRID_OFFSET_MULTIPLIER, -numberContainerSize * _GRID_OFFSET_MULTIPLIER);

            // Sets the position of the top numbers.
            _numberContainerTop.localPosition = new Vector2(
                _gridArea.localPosition.x,
                (_gridArea.sizeDelta.y / _TWO) + (_numberContainerTop.sizeDelta.y / _TWO) + _NUMBEROFFSET + _gridArea.localPosition.y);

            // Sets the position of the left numbers.
            _numberContainerLeft.localPosition = new Vector2(
                -(_gridArea.sizeDelta.x / _TWO) + -(_numberContainerLeft.sizeDelta.x / _TWO) + -_NUMBEROFFSET + _gridArea.localPosition.x,
                _gridArea.localPosition.y);
        }
    }
}
