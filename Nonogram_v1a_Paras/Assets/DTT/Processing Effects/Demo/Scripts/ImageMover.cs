using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.ProcessingEffects.Demo
{
    /// <summary>
    /// Displays the images and swaps them.
    /// </summary>
    public class ImageMover : MonoBehaviour
    {
        /// <summary>
        /// The different images to display.
        /// </summary>
        [SerializeField]
        private List<Sprite> _displayImages;

        /// <summary>
        /// The image component to change to current image.
        /// </summary>
        [SerializeField]
        private Image _decreaseImage;

        /// <summary>
        /// The image component to change to current image.
        /// </summary>
        [SerializeField]
        private Image _nextImage;

        /// <summary>
        /// Holds the original image of the shown one with the effect.
        /// </summary>
        [SerializeField]
        private Image _originalDecreaseImage;

        /// <summary>
        /// Holds the original image of the shown one with the effect.
        /// </summary>
        [SerializeField]
        private Image _originalNextImage;

        /// <summary>
        /// Tells which image was last used.
        /// </summary>
        private bool _toggleImage;

        /// <summary>
        /// The index of the current image.
        /// </summary>
        private int _currentImageIndex;

        /// <summary>
        /// Hold the last time the image was changed. Used to put a cooldown on it.
        /// </summary>
        private float _lastSwitchTime;

        /// <summary>
        /// Determens the minimum time between swapping images.
        /// </summary>
        private const float _COOLDOWN_TIME = 2;

        /// <summary>
        /// Starts the image quarantine.
        /// </summary>
        private void OnEnable() => StartCoroutine(SetNextImage());

        /// <summary>
        /// Sets the current image to the UI.
        /// </summary>
        private IEnumerator SetNextImage()
        {
            // Sets the next images.
            _decreaseImage.sprite = _nextImage.sprite;
            _originalDecreaseImage.sprite = _nextImage.sprite;

            _nextImage.sprite = _displayImages[_currentImageIndex];
            _originalNextImage.sprite = _displayImages[_currentImageIndex];

            // Sets the alpha of the new image.
            Color newColor = _nextImage.color;
            newColor.a = 1;
            _nextImage.color = newColor;
            _originalNextImage.color = newColor;

            // Sets the alpha of the previous image.
            Color previousColor = _decreaseImage.color;
            previousColor.a = 1;
            _decreaseImage.color = previousColor;
            _originalDecreaseImage.color = previousColor;

            // Fade the previous image.
            while (previousColor.a > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                previousColor.a -= Time.deltaTime;
                _decreaseImage.color = previousColor;
                _originalDecreaseImage.color = previousColor;
            }
        }

        /// <summary>
        /// Selects the current image index + 1.
        /// </summary>
        public void NextImage()
        {
            if (_lastSwitchTime > Time.time)
                return;
            _lastSwitchTime = Time.time + _COOLDOWN_TIME;

            _currentImageIndex++;
            if (_currentImageIndex >= _displayImages.Count)
                _currentImageIndex = 0;

            StartCoroutine(SetNextImage());
        }

        /// <summary>
        /// Selects the current image index - 1.
        /// </summary>
        public void PreviousImage()
        {
            if (_lastSwitchTime > Time.time)
                return;
            _lastSwitchTime = Time.time + _COOLDOWN_TIME;

            _currentImageIndex--;
            if (_currentImageIndex < 0)
                _currentImageIndex = _displayImages.Count - 1;

            StartCoroutine(SetNextImage());
        }
    }
}
