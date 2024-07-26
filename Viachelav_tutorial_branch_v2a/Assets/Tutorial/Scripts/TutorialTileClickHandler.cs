using DG.Tweening;
using DTT.Nonogram.Demo;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram
{
    public class TutorialTileClickHandler : MonoBehaviour
    {
        public NonogramTutorialManager manager;
        public TileTemp[] rowTiles;
        public GameObject[] highlighters;

        public bool hasArrows;
        public Image[] arrows;

        private void OnEnable()
        {
            //show the arrows with dealy and animate the arrows either by fillamout or scale up
            if(hasArrows)
            {
                AnimateArrows();
            }
        }

        public void OnClick(TileTemp tile)
        {
            if(tile.isCompulsory)
            {
                if (!tile.isFilled)
                {
                    GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueFound);
                    tile.FillTile();
                    if(manager.CheckRowCompletion(rowTiles))
                    {
                        foreach(GameObject highlight in highlighters)
                        {
                            highlight.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueWrong);
                Debug.Log("Show Warning Message!");
            }
        }
        private void AnimateArrows()
        {
            Sequence arrowSequence = DOTween.Sequence();
            arrowSequence.PrependInterval(1.1f);
            foreach (var arrow in arrows)
            {
                arrow.transform.localScale = Vector3.zero;

                // Scale the arrow from 0 to 1 over 0.5 seconds
                arrowSequence.Append(
                    arrow.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                );

                // Add a delay of 0.2 seconds between each arrow's animation
                arrowSequence.AppendInterval(0.2f);
            }
            arrowSequence.Play();
        }
    }
}
