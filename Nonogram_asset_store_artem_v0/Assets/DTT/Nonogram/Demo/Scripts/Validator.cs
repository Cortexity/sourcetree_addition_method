using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTT.MinigameBase.Timer;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Handles validating the Nongoram. Applies the hints to the Nonogram.
    /// </summary>
    public class Validator : MonoBehaviour
    {
        /// <summary>
        /// Reference to the NonogramManager. Used to complete the game.
        /// </summary>
        [SerializeField]
        private NonogramManager _manager;

        /// <summary>
        /// Reference to the timer object. Used to get the current time elapsed when validating.
        /// </summary>
        [SerializeField]
        private Timer _timer;

        /// <summary>
        /// Used for checking all the tiles and their states.
        /// </summary>
        private List<Tile> _allTiles;

        /// <summary>
        /// Stores all tiles to instantly fill the board.
        /// </summary>
        private List<TileBehaviour> _tiles;

        /// <summary>
        /// Adds the hint function to the hint event.
        /// </summary>
        private void OnEnable()
        {
            _allTiles = new List<Tile>();
            _tiles = new List<TileBehaviour>();
            _manager.HintUsed += GiveHint;
        }

        /// <summary>
        /// Removes the hint function to the hint event.
        /// </summary>
        private void OnDisable() => _manager.HintUsed -= GiveHint;

        /// <summary>
        /// Sets the current spawned in tiles to lists.
        /// </summary>
        /// <param name="allTiles">The list of tiles.</param>
        /// <param name="tiles">The tile objects.</param>
        public void SetTiles(List<Tile> allTiles, List<TileBehaviour> tiles)
        {
            _allTiles = allTiles;
            _tiles = tiles;
        }

        /// <summary>
        /// Lets the NonogramManager check if the Nonogram is correct or not.
        /// </summary>
        public void ValidateNonogram()
        {
            if (_manager.ValidateNonogram(_allTiles.ToArray(), _timer.TimePassed.Seconds))
                SetTilesToCorrectColor();
        }

        /// <summary>
        /// Gives a hint for the user.
        /// </summary>
        /// <param name="i">Needs to hold an int for the action. It is not being used.</param>
        public void GiveHint(int i) => MarkRandomIncorrectTile();

        /// <summary>
        /// Marks a random tile that is not in the correct state.
        /// </summary>
        private void MarkRandomIncorrectTile()
        {
            List<TileBehaviour> badTiles = new List<TileBehaviour>();
            foreach (var item in _tiles)
                if (item.Tile.CorrectStatus != item.Tile.CurrentStatus && item.Tile.CurrentMark != TileMark.WRONG)
                    badTiles.Add(item);

            if (badTiles.Count < 1)
                return;

            int randomTile = Random.Range(0, badTiles.Count);
            badTiles[randomTile].ChangeTileStatus(TileMark.WRONG);
        }

        /// <summary>
        /// Sets all the tiles to their correct state.
        /// </summary>
        public void SetTilesToCorrectColor()
        {
            foreach (var item in _tiles)
                item.SetTileToCorrect();
        }

        /// <summary>
        /// Sets all the tiles to their correct state over a short amount of time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator SetCorrectOverTime()
        {
            foreach (var item in _tiles)
            {
                yield return new WaitForEndOfFrame();
                item.SetTileToCorrect();
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.C))
                StartCoroutine(SetCorrectOverTime());
        }
    }
}
