using DTT.Nonogram.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTT.Nonogram
{
    [RequireComponent(typeof(Image))]
    public class TileTemp : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool isFilled = false;
        public bool isCompulsory = false;
        public Color MarkedTileColor;
        [HideInInspector]
        public Image TileImage;
        public GameObject XMark;
        public TutorialTileClickHandler tileClickHandler;
        public NonogramTutorialManager manager;

        private void Start()
        {
            TileImage = GetComponent<Image>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isFilled && !manager.isDraging)
            {
                manager.isDraging = true;
                tileClickHandler.OnClick(this);
            }
        }

        public void FillTile()
        {
            isFilled = true;
            TileImage.color = MarkedTileColor;
            XMark.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isFilled && manager.isDraging)
            {
                tileClickHandler.OnClick(this);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            manager.isDraging = false;
        }
    }
}
