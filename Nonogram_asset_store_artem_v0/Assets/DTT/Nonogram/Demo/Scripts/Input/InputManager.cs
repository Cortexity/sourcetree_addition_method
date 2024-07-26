using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Manages the input states for the Tiles
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// Current selected mark to use on tiles.
        /// </summary>
        public TileMark SelectedMark { get; private set; } = TileMark.EMPTY;

        /// <summary>
        /// True when a drag started.
        /// </summary>
        public bool StartedDrag { get; private set; }

        /// <summary>
        /// Current selected color to use on tiles.
        /// </summary>
        public Color CurrentSelectedColor { get; private set; } = Color.white;

        /// <summary>
        /// The state the first tile changed to when a drag started.
        /// </summary>
        public bool LastSelectedState { get; private set; }

        /// <summary>
        /// While this is true, use drag instead of drawing on the board.
        /// </summary>
        public bool UseDrag { get; private set; }

        /// <summary>
        /// The button to press when wanting to draw on the board.
        /// </summary>
        [SerializeField]
        private Button _drawDragButton;

        /// <summary>
        /// The scroll rect to turn on/off when wanting to move the UI.
        /// </summary>
        [SerializeField]
        private ScrollRect _scrollRect;

        /// <summary>
        /// The text object of the drag/move button.
        /// </summary>
        [SerializeField]
        private Image _dragMoveImage;

        /// <summary>
        /// Icon to show the player for dragging.
        /// </summary>
        [SerializeField]
        private Sprite _dragIcon;

        /// <summary>
        /// Icon to show the player for drawing.
        /// </summary>
        [SerializeField]
        private Sprite _drawIcon;

        /// <summary>
        /// Sets the draw/drag button listener.
        /// </summary>
        private void OnEnable() => _drawDragButton.onClick.AddListener(ToggleDragState);

        /// <summary>
        /// Removes the listener when diabling the object.
        /// </summary>
        private void OnDisable() => _drawDragButton.onClick.RemoveListener(ToggleDragState);

        /// <summary>
        /// Sets the current color and current mark.
        /// </summary>
        /// <param name="newColor">New color to set.</param>
        /// <param name="newMark">New mark to set.</param>
        public void SetNewSelection(Color newColor, TileMark newMark = TileMark.EMPTY)
        {
            SelectedMark = newMark;
            CurrentSelectedColor = newColor;
        }

        /// <summary>
        /// Sets the state if the drag has started.
        /// </summary>
        /// <param name="state">New state.</param>
        public void SetStartDrag(bool state) => StartedDrag = state;

        /// <summary>
        /// Holds the state of the first tile of the drag.
        /// </summary>
        /// <param name="state">New state.</param>
        public void SetDragStartState(bool state) => LastSelectedState = state;

        /// <summary>
        /// Enables/disables the drag functions;
        /// </summary>
        private void ToggleDragState()
        {
            UseDrag = !UseDrag;
            _scrollRect.enabled = UseDrag;
            _dragMoveImage.sprite = UseDrag ? _drawIcon : _dragIcon;
        }
    }
}
