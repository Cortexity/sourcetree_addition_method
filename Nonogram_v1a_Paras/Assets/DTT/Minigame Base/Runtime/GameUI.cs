using DTT.MinigameBase.LevelSelect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.MinigameBase.UI
{
    /// <summary>
    /// Standardized user interface for minigames.
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        /// <summary>
        /// The popup used for pausing and finishing.
        /// </summary>
        [SerializeField]
        private GamePopupUI _popup;

        public static GameUI instance;
        /// <summary>
        /// The button for pausing the game.
        /// </summary>
        [SerializeField]
        private Button _pauseButton;

        /// <summary>
        /// The game object on which the minigame is placed.
        /// </summary>
        [SerializeField]
        private GameObject _minigameGameObject;

        /// <summary>
        /// The minigame instance.
        /// </summary>
        private IMinigame _minigame;

        /// <summary>
        /// The part of the minigame that can be restarted.
        /// </summary>
        private IRestartable _restartable;

        /// <summary>
        /// The part of the minigame that will let us know when it's finished.
        /// </summary>
        private IFinishedable _finishedable;

        /// <summary>
        /// The level select currently active.
        /// </summary>

        [SerializeField]
        private LevelSelectHandlerBase _levelSelectHandler;

        [SerializeField] private Button _NextButton;
        public List<GameObject> Heart = new List<GameObject>();
        public int HeartCounter;

        public AudioSource Audio;
        public AudioClip WinClip;
        public AudioClip Buttonclick;
        public bool isPop = false;

        public void FullHeart()
        {
            HeartCounter = 3;
            for (int i = 0; i < Heart.Count; i++)
            {
                Heart[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Retrieves interface implementations.
        /// </summary>
        private void Awake()
        {
            instance = this;
            _minigame = _minigameGameObject.GetComponent<IMinigame>();
            _restartable = _minigameGameObject.GetComponent<IRestartable>();
            _finishedable = _minigameGameObject.GetComponent<IFinishedable>();
            _levelSelectHandler = FindObjectOfType<LevelSelectHandlerBase>();
            HeartCounter = 3;
        }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            _pauseButton.onClick.AddListener(PauseGame);
            _NextButton.onClick.AddListener(NextGame);
            _finishedable.Finished += FinishGame;
            _popup.ResumeButtonPressed += ResumeGame;
            _popup.RestartButtonPressed += RestartGame;
            _popup.HomeButtonPressed += ToHome;
        }

        /// <summary>
        /// Removes events.
        /// </summary>
        private void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(PauseGame);
            _NextButton.onClick.RemoveListener(NextGame);
            _finishedable.Finished -= FinishGame;
            _popup.ResumeButtonPressed -= ResumeGame;
            _popup.RestartButtonPressed -= RestartGame;
            _popup.HomeButtonPressed -= ToHome;
        }
        public bool isFinished = false;
        /// <summary>
        /// Sets the UI in a state for when the game finishes.
        /// </summary>
        private void Start()
        {
            _popup.Show(false);
        }
        private void Update()
        {
            Debug.Log("gggggggggggggggggggggg " + isFinished);
        }
        public void FinishGame()
        {
            isFinished = true;
            StartCoroutine(FinishWait());
        }
        IEnumerator FinishWait()
        {
            Debug.Log("Finish");
            _popup.SetTitleToFinished();
            _popup.EnableResumeButton(false);
            yield return new WaitForSeconds(3f);
            Audio.PlayOneShot(WinClip);
            _popup.Show(true);
        }
        /// <summary>
        /// Sets the UI in a state for when the game restarts.
        /// </summary>
        
        private void RestartGame()
        {
            Audio.PlayOneShot(Buttonclick);
            isFinished = false;
            _minigame.Continue();
            _restartable.Restart();
            _popup.Show(false);
            _NextButton.gameObject.SetActive(true);
            FullHeart();
            StartCoroutine(Gamepop());
        }

        /// <summary>
        /// Sets the UI in a state for when the game resumes.
        /// </summary>
        private void ResumeGame()
        {
            Audio.PlayOneShot(Buttonclick);
            isFinished = false;
            _minigame.Continue();
            _popup.Show(false);
            _popup.EnableNextLevelButton(true);
            StartCoroutine(Gamepop());
        }

        /// <summary>
        /// Sets the UI in a state for when the game resumes.
        /// </summary>
        private void PauseGame()
        {
            Audio.PlayOneShot(Buttonclick);
            isFinished = false;
            Debug.Log("Pause");
            _minigame.Pause();
            _popup.Show(true);
            _popup.SetTitleToPaused();
            _popup.EnableResumeButton(true);
            _popup.EnableNextLevelButton(false);
        }
        /// <typeparam name="TConfig">The configuration type of your minigame.</typeparam>
        /// <typeparam name="TResult">The result type of your minigame.</typeparam>
        /// <typeparam name="TMinigame">The minigame manager class.</typeparam>

        private void NextGame()
        {
            Audio.PlayOneShot(Buttonclick);
            isFinished = false;
            _popup.Show(false);
            _levelSelectHandler.Nextss();
            // LevelButton.Instance.OnButtonClicked();
            FullHeart();
            StartCoroutine(Gamepop());
        }
        /// <summary>
        /// Sets the UI in a state for when the game goes back to home.
        /// </summary>
        private void ToHome()
        {
            Audio.PlayOneShot(Buttonclick);
            isFinished = false;
            _NextButton.gameObject.SetActive(true);
            _popup.Show(false);
            _levelSelectHandler.ShowLevelSelect();
            FullHeart();
            StartCoroutine(Gamepop());
        }
        IEnumerator Gamepop()
        {
            isPop = true;
            yield return new WaitForSeconds(0.5f);
            isPop = false;
        }
    }
}