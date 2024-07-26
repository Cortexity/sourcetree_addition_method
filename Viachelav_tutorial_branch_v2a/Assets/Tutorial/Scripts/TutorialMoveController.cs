using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMoveController : MonoBehaviour
{
    [SerializeField]
    private float _zoomSpeed = .1f;

    [SerializeField]
    private RectTransform _uiToMove;

    private float zoomScale = 1;

    [SerializeField]
    private ScrollRect _scrollView;

    public NonogramTutorialManager _manager;

    private float initialTouchDistance = 0f;

    private void Update()
    {
        if(_manager.useDrag)
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        bool enableScroll;
        float currentTouchDistance = 0f;
        if (Input.touchCount > 1)
        {
            Debug.Log("Zooming");
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            currentTouchDistance = Vector2.Distance(touchZero.position, touchOne.position);

            if (initialTouchDistance == 0f)
            {
                initialTouchDistance = currentTouchDistance;
            }

            // Determine zoom direction based on change in touch distance
            if (currentTouchDistance > initialTouchDistance)
            {
                _manager.isZoomedIn = true;
            }
            else if (currentTouchDistance < initialTouchDistance)
            {
                _manager.isZoomedOut = true;
            }

            initialTouchDistance = currentTouchDistance;

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
