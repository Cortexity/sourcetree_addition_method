using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Controls the movement of the grid when drawing or zooming in/out.
    /// </summary>
    public class MoveController : MonoBehaviour
    {
        /// <summary>
        /// Determens the speed of the zoom.
        /// </summary>
        [SerializeField]
        private float _zoomSpeed = .1f;

        /// <summary>
        /// The UI that gets moved.
        /// </summary>
        [SerializeField]
        private RectTransform _uiToMove;

        /// <summary>
        /// Reference for checking if the player started drag on tiles or not.
        /// </summary>
        [SerializeField]
        private InputManager _inputManager;

        /// <summary>
        /// Should be true when using a horizontal scene. Sets the bounds of when moving the Nonogram.
        /// </summary>
        [SerializeField]
        private bool _isLandscape;

        /// <summary>
        /// Holds the scale of the UI to zoom in on.
        /// </summary>
        private float zoomScale = 1;

        /// <summary>
        /// handles the movement of the UI.
        /// </summary>
        [SerializeField]
        private ScrollRect _scrollView;

        /// <summary>
        /// Checks if the player state allows zooming.
        /// </summary>
        private void Update()
        {
            if (_inputManager.UseDrag)
                Zoom();
        }

        /// <summary>
        /// Zoom in/out on the Nonogram.
        /// </summary>
        private void Zoom()
        {
            bool enableScroll;
            if(Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float previousMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                zoomScale = Mathf.Clamp(zoomScale + (currentMagnitude - previousMagnitude) * _zoomSpeed * 0.01f, .5f, 2.5f);
                enableScroll = false;
            }
            else
            {
                zoomScale = Mathf.Clamp(zoomScale + Input.mouseScrollDelta.y * _zoomSpeed, .5f, 2.5f);
                enableScroll = true;
            }

            _scrollView.enabled = enableScroll;
            _uiToMove.transform.localScale = Vector3.one * zoomScale;
        }
    }
}
