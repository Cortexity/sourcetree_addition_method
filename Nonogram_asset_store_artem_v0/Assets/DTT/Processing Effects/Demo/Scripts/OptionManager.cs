using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.ProcessingEffects.Demo
{
    /// <summary>
    /// Gives the user control over the posterize/pixelate value with a slider.
    /// </summary>
    public class OptionManager : MonoBehaviour
    {
        /// <summary>
        /// The material used with the effects.
        /// </summary>
        [SerializeField]
        protected Material _optionViewMat;

        /// <summary>
        /// The slider to increase/decrease the posterize of effect.
        /// </summary>
        [SerializeField]
        protected Slider _posterizeSlider;

        /// <summary>
        /// The slider to increase/decrease the pixelate of effect.
        /// </summary>
        [SerializeField]
        private Slider _pixelSlider;

        /// <summary>
        /// The slider to increase/decrease the grayscale of effect.
        /// </summary>
        [SerializeField]
        protected Slider _grayscaleSlider;

        /// <summary>
        /// Calls the inncrease effect.
        /// </summary>
        private void Update() => OptionEffectIncrease();

        /// <summary>
        /// Increases the amount the enabled effect has.
        /// </summary>
        protected virtual void OptionEffectIncrease()
        {
            if (_posterizeSlider != null)
                _optionViewMat.SetFloat("_aPower", 1.5f + (_posterizeSlider.value * .1f));
            if(_pixelSlider != null)
                _optionViewMat.SetFloat("_Pixelazition", 60 + (_pixelSlider.value * 50f));
            if (_grayscaleSlider != null)
                _optionViewMat.SetFloat("_Grayscale", _grayscaleSlider.value);
        }
    }
}