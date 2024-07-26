using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using DTT.Nonogram;
using UnityEngine.UI;
using DTT.Nonogram.Demo;
using DTT.MinigameBase.UI;

public class NonogramTutorialManager : MonoBehaviour
{
    public GameObject HowToPlay;
    public GameObject[] tutorialSteps;
    public Transform handPointer;
    public Vector3[] handEndPositions;
    public Vector3[] handStartPositions;
    public GameObject[] ticks;

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

    private int currentStep = 0;

    public bool isDraging = false;

    public bool useDrag = false;

    public bool isZoomedIn = false;
    public bool isZoomedOut = false;

    Sequence clickHandSequence;

    void Start()
    {
        StartTutorial();
    }

    void StartTutorial()
    {
        HowToPlay.transform.localScale = new Vector3(0, 1, 1);

        Sequence tutorialSequence = DOTween.Sequence();

        tutorialSequence
            .Append(HowToPlay.transform.DOScaleX(1, 1.5f))
            .AppendInterval(0.5f)
            .OnComplete(() =>
            {
                HowToPlay.SetActive(false);
                ShowCurrentStep();
            });
    }

    void ShowCurrentStep()
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            if(i == currentStep)
            {
                tutorialSteps[i].SetActive(true);
                tutorialSteps[i].transform.localScale = new Vector3(1, 0, 1);
                tutorialSteps[i].transform.DOScaleY(1, 0.5f);
            }
        }
        if(currentStep <= 1)
        {
            handPointer.gameObject.SetActive(true);
            handPointer.localPosition = handStartPositions[currentStep];
            AnimateHand();
        }
        if(currentStep == 4)
        {
            handPointer.gameObject.SetActive(true);
            handPointer.localPosition = handStartPositions[3];
            AnimateStep4Hand();
        }
        if(currentStep == 5)
        {
            handPointer.gameObject.SetActive(true);
            handPointer.localPosition = handStartPositions[2];
            AnimateStep6Hand();
        }
        if(currentStep == 6)
        {
            ZoomInTutorial();
        }
    }

    void AnimateHand()
    {
        if (currentStep < handEndPositions.Length)
        {
            handPointer.DOKill();
            handPointer.DOLocalMove(handEndPositions[currentStep], 2f).SetLoops(-1, LoopType.Yoyo);

        }
    }
    void AnimateStep4Hand()
    {
        tutorialSteps[5].SetActive(true);
        tutorialSteps[5].transform.localScale = new Vector3(1, 0, 1);
        tutorialSteps[5].transform.DOScaleY(1, 0.5f);

        handPointer.DOKill();

        if(clickHandSequence !=  null)
        {
            clickHandSequence.Kill();
        }

        clickHandSequence = DOTween.Sequence();

        // Move the hand pointer to the end position
        clickHandSequence.Append(handPointer.DOLocalMove(handEndPositions[3], 1f).SetEase(Ease.InOutSine))
                          // Add the scaling animation: zoom in and zoom out
                          .Append(handPointer.DOScale(0.8f, 0.5f).SetEase(Ease.OutQuad))
                          .Append(handPointer.DOScale(1.3f, 0.5f).SetEase(Ease.OutQuad))
                          .SetLoops(-1, LoopType.Restart);
    }
    void AnimateStep6Hand()
    {
        handPointer.DOKill();

        if (clickHandSequence != null)
        {
            clickHandSequence.Kill();
        }

        clickHandSequence = DOTween.Sequence();

        // Move the hand pointer to the end position
        clickHandSequence.Append(handPointer.DOLocalMove(handEndPositions[2], 1f).SetEase(Ease.InOutSine))
                          // Add the scaling animation: zoom in and zoom out
                          .Append(handPointer.DOScale(0.8f, 0.5f).SetEase(Ease.OutQuad))
                          .Append(handPointer.DOScale(1.3f, 0.5f).SetEase(Ease.OutQuad))
                          .SetLoops(-1, LoopType.Restart);
    }
    public void StopAnimateHand()
    {
        handPointer.DOKill();
        handPointer.gameObject.SetActive(false);
    }
    void OnStepComplete()
    {
        GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.LineComplete);
        ticks[currentStep].SetActive(true);
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
            if (tile.isCompulsory && !tile.isFilled) return false;
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