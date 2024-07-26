using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using DTT.Nonogram.Demo;
using DTT.MinigameBase.UI;
using System.Collections.Generic;
using System;

public class NonogramTutorialManager : MonoBehaviour
{
    public GameObject Title;
    public Text titleText;

    public string[] titles;

    public TutorialStep[] tutorialStep;

    public bool canPlace;

    public GameObject Buttons;

    public GameObject zoomIn;
    public GameObject zoomOut;

    public GameObject Complete;

    public GameObject[] tutorialSteps;

    public Transform handPointer;

    public List<HandStepPositions> handStepPositions = new List<HandStepPositions>();

    public Vector3[] handEndPositions;
    public Vector3[] handStartPositions;
    //public GameObject[] ticks;

    public Sprite dragIcon;
    public Sprite drawIcon;
    public Image _dragMoveImage;
    public ScrollRect _scrollRect;

    public Transform[] zoominArrows;
    public Transform[] zoomoutArrows;
    public Vector3 zoomleftArrowStartPos;
    public Vector3 zoomleftArrowEndPos;

    public Vector3 zoomRightArrowStartPos;
    public Vector3 zoomRightArrowEndPos;
    [SerializeField]
    private int currentStep = 0;

    public bool isDraging = false;

    public bool useDrag = false;

    public bool isZoomedIn = false;
    public bool isZoomedOut = false;

    Sequence clickHandSequence;
    Sequence stepSequence;
    void Start()
    {
        StartTutorial();
    }

    void StartTutorial()
    {
        canPlace = true;
        titleText.text = "";
        Title.transform.localScale = new Vector3(0, 1, 1);

        Title.transform.DOScaleX(1, 1f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                titleText.DOText("HOW TO PLAY NONOGRAM!", 2f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        ShowCurrentStep();
                    });                    
                });
            });
        });
    }

    void ShowCurrentStep()
    {
        if (currentStep == 4)
        {
            RectTransform titleRect = Title.GetComponent<RectTransform>();
            if (titleRect != null)
            {
                titleRect.DOAnchorPos(new Vector2(-720, -100), 0.5f).SetEase(Ease.InOutSine);
                titleRect.DOSizeDelta(new Vector2(500, 700), 0.5f).SetEase(Ease.InOutSine);
            }

        }
        titleText.text = "";
        titleText.DOText(titles[currentStep], 2f);
        
        /*for (int i = 0; i < tutorialSteps.Length; i++)
        {
            if(i == currentStep)
            {
                tutorialSteps[i].SetActive(true);
                tutorialSteps[i].transform.localScale = new Vector3(1, 0, 1);
                tutorialSteps[i].transform.DOScaleY(1, 0.5f);
            }
        }*/
        if(currentStep <= 4)
        {
            tutorialSteps[currentStep].SetActive(true);
            tutorialSteps[currentStep].transform.localScale = new Vector3(1, 0, 1);
            tutorialSteps[currentStep].transform.DOScaleY(1, 0.5f);
            StepAnimation();
        }
        if(currentStep == 5)
        {
            AnimateHand();
        }
        if(currentStep == 6)
        {
            canPlace = false;
            zoomIn.SetActive(true);
            ZoomInTutorial();
        }
        if(currentStep == 7)
        {
            zoomIn.SetActive(false);
            zoomOut.SetActive(true);
        }
        if(currentStep == 8)
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                Complete.SetActive(true);
            });
        }
    }

    void StepAnimation()
    {
        if (stepSequence != null && stepSequence.IsActive())
        {
            stepSequence.Kill();
        }
        stepSequence = DOTween.Sequence();
        stepSequence.AppendInterval(1f);

        // Fill the circles twice with delay
        foreach (var circleImage in tutorialStep[currentStep].circles)
        {
            stepSequence.Append(circleImage.DOFillAmount(1f, 0.5f).SetEase(Ease.InOutSine))
                        .AppendInterval(0.5f)
                        .Append(circleImage.DOFillAmount(0f, 0.1f).SetEase(Ease.InOutSine))
                        .AppendInterval(0.5f)
                        .Append(circleImage.DOFillAmount(1f, 0.5f).SetEase(Ease.InOutSine));
        }

        // Show highlights
        foreach (var highlight in tutorialStep[currentStep].highlights)
        {
            if(currentStep == 4)
            {
                Buttons.SetActive(true);
                Buttons.transform.localScale = new Vector3(1, 0, 1);
                Buttons.transform.DOScaleY(1, 0.5f);
            }
            stepSequence.AppendCallback(() => highlight.SetActive(true)).OnComplete(() =>
            {
                AnimateStepHand();
                
            });
        }

        stepSequence.Play();
    }
    void AnimateStepHand()
    {
        if (currentStep < handStepPositions.Count && tutorialStep[currentStep].tileNo < handStepPositions[currentStep].positions.Length)
        {
            handPointer.DOKill();
            handPointer.localPosition = handStepPositions[currentStep].positions[tutorialStep[currentStep].tileNo];
            handPointer.gameObject.SetActive(true);
            if(currentStep <= 5)
            {
                tutorialStep[currentStep].ActivateTile();
            }
            
            if (stepSequence != null && stepSequence.IsActive())
            {
                stepSequence.Kill();
            }
            stepSequence = DOTween.Sequence();

            stepSequence.Append(handPointer.DOScale(0.8f, 0.5f).SetEase(Ease.OutQuad))
                              .Append(handPointer.DOScale(1.3f, 0.5f).SetEase(Ease.OutQuad))
                              .AppendInterval(1f)
                              .SetLoops(-1, LoopType.Yoyo);

            clickHandSequence.Play();
        }
    }
    

    void AnimateHand()
    {
        if (currentStep < handStepPositions.Count)
        {
            handPointer.DOKill();
            handPointer.localPosition = handStepPositions[currentStep].positions[0];
            handPointer.gameObject.SetActive(true);

            if (stepSequence != null && stepSequence.IsActive())
            {
                stepSequence.Kill();
            }
            stepSequence = DOTween.Sequence();

            stepSequence.Append(handPointer.DOScale(0.8f, 0.5f).SetEase(Ease.OutQuad))
                              .Append(handPointer.DOScale(1.3f, 0.5f).SetEase(Ease.OutQuad))
                              .AppendInterval(1f)
                              .SetLoops(-1, LoopType.Yoyo);

            clickHandSequence.Play();
        }
    }
    
    
    public void StopAnimateHand()
    {
        handPointer.DOKill();
        handPointer.gameObject.SetActive(false);
    }
    void OnStepComplete()
    {
        GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.LineComplete);
        if(currentStep < 5)
        {
            foreach(var tick in tutorialStep[currentStep].ticks)
            {
                tick.SetActive(true);
            }
        }
        NextStep();
    }
    void NextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            ShowCurrentStep();
        }
        else
        {
            EndTutorial();
        }
    }

    public void EndTutorial()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        SceneManager.UnloadSceneAsync("Tutorial");
    }

    public void RestartTutorial()
    {
        currentStep = 0;
        ShowCurrentStep();
    }

    public bool CheckRowCompletion(TileTemp[] rowTiles)
    {        
        foreach (TileTemp tile in rowTiles)
        {
            if (tile.isCompulsory && !tile.isFilled)
            {
                AnimateStepHand();
                return false;
            }
        }
        foreach (TileTemp tile in rowTiles)
        {
            if(!tile.isCompulsory)
            {
                tile.XMark.SetActive(true);
            }
        }
        handPointer.gameObject.SetActive(false);
        AnimateTileColors(rowTiles);
        return true;
    }

    void AnimateTileColors(TileTemp[] rowTiles)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = rowTiles.Length - 1; i >= 0; i--)
        {
            int index = i;
            Color originalColor = rowTiles[index].TileImage.color;
            sequence.Insert((rowTiles.Length - 1 - index) * 0.2f,
                rowTiles[index].TileImage.DOColor(UnityEngine.Random.ColorHSV(), 0.5f).OnComplete(() =>
                {
                    rowTiles[index].TileImage.DOColor(originalColor, 0.5f);
                }));
        }

        sequence.OnComplete(OnStepComplete);
    }

    public void UseDrag()
    {
        if (currentStep < 5) return;
        GridRenderer.instance.Audio.PlayOneShot(GameUI.instance.Buttonclick);
        useDrag = !useDrag;
        _scrollRect.enabled = useDrag;
        _dragMoveImage.sprite = useDrag ? drawIcon : dragIcon;
        handPointer.gameObject.SetActive(false);
        tutorialSteps[currentStep].SetActive(false);

        NextStep();
    }
    
    void ZoomInTutorial()
    {
        int zoomInPlayCount = 0;
        zoominArrows[0].localPosition = zoomleftArrowStartPos;
        zoominArrows[1].localPosition = zoomRightArrowStartPos;

        // Create a sequence to handle the animation
        Sequence zoomSequence = DOTween.Sequence();

        // Add initial delay
        zoomSequence.AppendInterval(0.5f);

        // Animate the arrows to move further away
        zoomSequence.Append(zoominArrows[0].DOLocalMove(zoomleftArrowEndPos, 1).SetEase(Ease.InOutQuad));
        zoomSequence.Join(zoominArrows[1].DOLocalMove(zoomRightArrowEndPos, 1).SetEase(Ease.InOutQuad));

        // Add interval delay and repeat the animation
        zoomSequence.AppendInterval(0.15f);
        zoomSequence.SetLoops(-1, LoopType.Yoyo);

        // On complete of each loop, increment the counter and check if it has played 3 times
        zoomSequence.OnStepComplete(() =>
        {
            zoomInPlayCount++;
            if (zoomInPlayCount >= 3 && isZoomedIn)
            {
                // Stop the zoom-in animation and start the zoom-out animation
                zoomSequence.Kill();
                NextStep();
                ZoomOutTutorial();
            }
        });

        // Play the sequence
        zoomSequence.Play();
    }
    void ZoomOutTutorial()
    {
        tutorialSteps[6].SetActive(false);
        int zoomOutPlayCount = 0;
        zoomoutArrows[0].localPosition = zoomleftArrowEndPos;
        zoomoutArrows[1].localPosition = zoomRightArrowEndPos;

        // Create a sequence to handle the animation
        Sequence zoomSequence = DOTween.Sequence();

        // Add initial delay
        zoomSequence.AppendInterval(0.5f);

        // Animate the arrows to move further away
        zoomSequence.Append(zoomoutArrows[0].DOLocalMove(zoomleftArrowStartPos, 1).SetEase(Ease.InOutQuad));
        zoomSequence.Join(zoomoutArrows[1].DOLocalMove(zoomRightArrowStartPos, 1).SetEase(Ease.InOutQuad));

        // Add interval delay and repeat the animation
        zoomSequence.AppendInterval(0.15f);
        zoomSequence.SetLoops(-1, LoopType.Yoyo);

        // On complete of each loop, increment the counter and check if it has played 3 times
        zoomSequence.OnStepComplete(() =>
        {
            zoomOutPlayCount++;
            if (zoomOutPlayCount >= 3 && isZoomedOut)
            {
                // Stop the zoom-in animation and start the zoom-out animation
                zoomSequence.Kill();
                NextStep();
            }
        });

        // Play the sequence
        zoomSequence.Play();
    }
}

[Serializable]
public class HandStepPositions
{
    public Vector3[] positions;
}