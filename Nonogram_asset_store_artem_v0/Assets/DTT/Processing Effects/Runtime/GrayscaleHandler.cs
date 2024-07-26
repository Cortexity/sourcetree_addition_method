using System.Collections.Generic;
using UnityEngine;

namespace DTT.ProcessingEffects
{
    /// <summary>
    /// Handles converting colors to black and white.
    /// </summary>
    public class GrayscaleHandler
    {
        /// <summary>
        /// Turns the given color into black and white based on the grayscale of the given color.
        /// </summary>
        /// <param name="pixel">The color to convert.</param>
        /// <param name="grayscale">The threshold to ignore a color and return white.</param>
        /// <returns>The given color in black/white.</returns>
        public static Color ToBlackWhite(Color pixel, float grayscale)
        {
            if (grayscale >= pixel.grayscale)
                pixel = new Color(pixel.grayscale, pixel.grayscale, pixel.grayscale, pixel.a);
            else
                pixel = Color.white;

            return pixel;
        }

        /// <summary>
        /// Turn the given color array into black and white based on the grayscale of the given colors.
        /// </summary>
        /// <param name="pixels">The colors to convert.</param>
        /// <param name="grayscale">The threshold to ignore a color and return white.</param>
        /// <returns>The given color array in black/white.</returns>
        public static Color[] ToBlackWhite(Color[] pixels, float grayscale)
        {
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = ToBlackWhite(pixels[i], grayscale);

            return pixels;
        }

        /// <summary>
        /// Turns the given texture into a black/white texture.
        /// </summary>
        /// <param name="texture">The texture to convert.</param>
        /// <param name="grayscale">The threshold to ignore a color and return white.</param>
        /// <returns>The given texture in black/white.</returns>
        public static Texture2D ToBlackWhite(Texture2D texture, float grayscale)
        {
            Texture2D newTexture = new Texture2D(texture.width, texture.height);
            newTexture.SetPixels(ToBlackWhite(texture.GetPixels(), grayscale));
            newTexture.Apply();

            return newTexture;
        }

        /// <summary>
        /// Grays out a color based on the grayscale.
        /// </summary>
        /// <param name="color">The color to gray out.</param>
        /// <param name="grayscale">The grayscale threshold.</param>
        /// <returns>A color grayed out based on the grayscale.</returns>
        public static Color AddGrayscale(Color color, float grayscale)
        {
            float averageColor = (color.r + color.g + color.b) / 3;
            return Color.Lerp(color, new Color(averageColor, averageColor, averageColor, color.a), grayscale);
        }

        /// <summary>
        /// Makes the color array's pixels fade into gray based on the given grayscale.
        /// </summary>
        /// <param name="colors">The color array to modify.</param>
        /// <param name="grayscale">The amount of gray used.</param>
        /// <returns>A color array grayed out based on the grayscale.</returns>
        public static Color[] AddGrayscale(Color[] colors, float grayscale)
        {
            List<Color> modifiedColors = new List<Color>();

            foreach (Color color in colors)
                modifiedColors.Add(AddGrayscale(color, grayscale));

            return modifiedColors.ToArray();
        }

        /// <summary>
        /// Makes the texture fade into gray based on the given grayscale.
        /// </summary>
        /// <param name="texture">The texture to modify.</param>
        /// <param name="grayscale">The amount of gray used.</param>
        /// <returns>A texture grayed out based on the grayscale.</returns>
        public static Texture2D AddGrayscale(Texture2D texture, float grayscale)
        {
            Color[] newColors = AddGrayscale(texture.GetPixels(), grayscale);

            Texture2D newTexture = new Texture2D(texture.width, texture.height);
            newTexture.SetPixels(newColors);
            newTexture.Apply();

            return newTexture;
        }

        /// <summary>
        /// Makes a color white if below the grayscale.
        /// </summary>
        /// <param name="color">The color to modify.</param>
        /// <param name="grayscale">The grayscale threshold.</param>
        /// <returns>A color based on the grayscale.</returns>
        public static Color ExcludeGrayscale(Color color, float grayscale)
        {
            if (grayscale >= color.grayscale)
                return color;
            else
                return Color.white;
        }

        /// <summary>
        /// Makes the pixels that have a grayscale smaller than the given grayscale disappear.
        /// </summary>
        /// <param name="colors">The array of colors to modify.</param>
        /// <param name="grayscale">The grayscale threshold to remove pixels.</param>
        /// <returns>The color array without the pixels that have less grayscale than the given grayscale.</returns>
        public static Color[] ExcludeGrayscale(Color[] colors, float grayscale)
        {
            List<Color> modifiedColors = new List<Color>();

            foreach (Color color in colors)
                modifiedColors.Add(ExcludeGrayscale(color, grayscale));

            return modifiedColors.ToArray();
        }

        /// <summary>
        /// Makes the pixels that have a grayscale smaller than the given grayscale disappear.
        /// </summary>
        /// <param name="texture">The texture to modify.</param>
        /// <param name="grayscale">The grayscale threshold to remove pixels.</param>
        /// <returns>The texture without the pixels that have less grayscale than the given grayscale.</returns>
        public static Texture2D ExcludeGrayscale(Texture2D texture, float grayscale)
        {
            Color[] newColors = ExcludeGrayscale(texture.GetPixels(), grayscale);

            Texture2D newTexture = new Texture2D(texture.width, texture.height);
            newTexture.SetPixels(newColors);
            newTexture.Apply();

            return newTexture;
        }
    }
}