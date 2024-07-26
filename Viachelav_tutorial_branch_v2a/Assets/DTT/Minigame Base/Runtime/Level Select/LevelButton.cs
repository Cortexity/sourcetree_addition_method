using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DTT.MinigameBase.LevelSelect
{
    /// <summary>
    /// UI handler component for the buttons in the level overview that can be pressed to start a level.
    /// </summary>
    public class LevelButton : MonoBehaviour
    {
        public static LevelButton Instance;
        /// <summary>
        /// Called when the button is pressed.
        /// </summary>
        public event Action Pressed;

        /// <summary>
        /// Called when the button is pressed and passed itself with the callback.
        /// </summary>
        public event Action<LevelButton> PressedThis;

        /// <summary>
        /// The text component used to display the level number.
        /// </summary>
        [SerializeField]
        [Tooltip("The text component used to display the level number.")]
        public Text _levelNumberText;

        /// <summary>
        /// The parent object of the star images.
        /// </summary>
        [SerializeField]
        [Tooltip("The parent object of the star images.")]
        public Transform _starsParent;

        /// <summary>
        /// The star images that are used to display the score.
        /// </summary>
        [SerializeField]
        [Tooltip("The star images that are used to display the score.")]
        public Image[] _stars;

        /// <summary>
        /// The image of the lock, used to display when the level is locked.
        /// </summary>
        [SerializeField]
        [Tooltip("The image of the lock, used to display when the level is locked.")]
        public Image _lock;

        /// <summary>
        /// The button component for retrieving callbacks when the level button is clicked on.
        /// </summary>
        [SerializeField]
        [Tooltip("The button component for retrieving callbacks when the level button is clicked on.")]
        public Button _button;

        /// <summary>
        /// The integer value of the level number this button displays.
        /// </summary>
        public int _levelNumber;
        public AudioSource _audiosource;
        public AudioClip _BtnClick;

     
        /// <summary>
        /// The integer value of the level number this button displays.
        /// </summary>

        private void Awake()
        {
            Instance = this;
        }
        public int LevelNumber
        {

            get => _levelNumber;
            set
            {
                _levelNumber = value;
                _levelNumberText.text = value.ToString();
            }
        }

        /// <summary>
        /// Set the level to be locked.
        /// </summary>
        public void SetLocked(bool state = true)
        {
            _lock.enabled = state;
            _starsParent.gameObject.SetActive(!state);
            _button.interactable = !state;
        }

        /// <summary>
        /// Set the amount of stars to be active.
        /// </summary>
        /// <param name="amount">The amount of stars to be active.</param>
        public void SetStarsAmount(int amount)
        {
            SetLocked(false);

            _starsParent.gameObject.SetActive(true);
            for (int i = 0; i < _stars.Length; i++)
                _stars[i].enabled = i < amount;
        }

        /// <summary>
        /// Subscribes listeners.
        /// </summary>
        private void OnEnable() => _button.onClick.AddListener(OnButtonClicked);

        /// <summary>
        /// Removes listeners.
        /// </summary>
        private void OnDisable() => _button.onClick.RemoveListener(OnButtonClicked);
    
        /// <summary>
        /// Called when the button is clicked, and invokes this API's events.
        /// </summary>
        public void OnButtonClicked()
        {
            _audiosource.PlayOneShot(_BtnClick);
            if (PlayerPrefs.GetInt("TutorialCompleted", 0) == 0)
            {
                SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
                return;
            }
            Pressed?.Invoke();
            PressedThis?.Invoke(this);
        }
      
    }
}