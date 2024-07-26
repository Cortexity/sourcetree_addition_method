using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Used to change te color of the buttons.
    /// </summary>
    public class ColorButton : MonoBehaviour
    {
        /// <summary>
        /// The image that is used to display the color of the button.
        /// </summary>
        [SerializeField]
        private Image _fillImage;

        /// <summary>
        /// Changes the color of the fill area.
        /// </summary>
        /// <param name="newColor">The new color the area will have.</param>
        public void ChangeColor(Color newColor) => _fillImage.color = newColor;
    }
}
