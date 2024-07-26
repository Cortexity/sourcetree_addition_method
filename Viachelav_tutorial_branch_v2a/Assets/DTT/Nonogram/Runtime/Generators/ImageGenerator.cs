using System;
using UnityEngine;
using DTT.ProcessingEffects;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace DTT.Nonogram
{
    /// <summary>
    /// Generates a Nonogram based on the given image.
    /// </summary>
    [Serializable]
    public class ImageGenerator : INonogramGenerator
    {
        /// <summary>
        /// The image used for generating the Nonogram.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Sets the InputType used for this configuration.
        /// </summary>
        public SelectionType InputType;

        /// <summary>
        /// Generates a Nonogram based on the image.
        /// </summary>
        /// <param name="config">The Nonogram configuration settings to use.</param>
        /// <returns>A generated Nonogram based on the given image.</returns>
        public Color[] Generate(NonogramConfig config)
        {
            if (Texture == null)
            {
                Debug.Log("Please provide an image to the nonogram configuration");
                return null;
            }

            Texture2D image = ResizeHandler.Resize(Texture, config.NonogramSettings.GridSize);

            if (config.PosterizeSettings.PosterizeEnabled)
                image = PosterizeHandler.Posterize(image, config.PosterizeSettings.Level);                

            if (config.ColorSettings.UseColors)
                image = GrayscaleHandler.AddGrayscale(image, 1 - config.ColorSettings.GrayscaleColor);
            else
                image = GrayscaleHandler.AddGrayscale(image, 1);

            Color[] pixels = image.GetPixels();

            if (config.ColorSettings.GrayscaleThreshold < 1 || config.ImageGenerator.InputType == SelectionType.DEFAULT_TOGGLE)
            {
                pixels = GrayscaleHandler.ExcludeGrayscale(pixels, config.ColorSettings.GrayscaleThreshold);

                if(config.ImageGenerator.InputType == SelectionType.SELECT_COLOR)
                    pixels = pixels.Select(pixels => pixels == Color.white ? config.ColorSettings.DefaultColor : pixels).ToArray();
                else
                    pixels = pixels.Select(pixels => pixels == Color.white ? config.ColorSettings.DefaultColor : config.ColorSettings.ToggleColor).ToArray();
            }

            if (config.ColorSettings.UseColors && config.ColorSettings.ColorIntensity != 1)
                pixels = MultiplyColor(pixels, config.ColorSettings.ColorIntensity);

            return pixels;
        }

        /// <summary>
        /// Multiplies the color of the given texture.
        /// </summary>
        /// <param name="texture">The texture to multiply the colors of.</param>
        /// <param name="colorMultiplier">The amount to multiply the color with.</param>
        /// <returns>A texture that is brighter/darker.</returns>
        private Color[] MultiplyColor(Color[] texture, float colorMultiplier)
        {
            Color[] pixels = texture;
            Color[] newPixels = pixels.Select(item => item * colorMultiplier).ToArray();

            return newPixels;
        }
    }
}