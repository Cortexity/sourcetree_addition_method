using UnityEngine;

namespace DTT.ProcessingEffects
{
    /// <summary>
    /// Handles pixelating textures.
    /// </summary>
    public class PixelateHandler 
    {
        /// <summary>
        /// Pixelates a color array.
        /// </summary>
        /// <param name="colors">The color array to pixelize.</param>
        /// <param name="pixelCount">The pixel count on the x an y.</param>
        /// <returns>The color array pixelized.</returns>
        public static Color[] Pixelate(Color[] colors, int pixelCount)
        {
            if (pixelCount < 1)
                return colors;

            float totalCount = colors.Length / pixelCount;

            Color[] pixelColors = new Color[(int)totalCount];

            for (int i = 0; i < pixelColors.Length; i++)
            {
                Color pixelColor = colors[Mathf.Clamp(i * pixelCount, 0, colors.Length - 1)];
                pixelColor *= pixelCount;
                pixelColor = new Color(Mathf.Floor(pixelColor.r), Mathf.Floor(pixelColor.g), Mathf.Floor(pixelColor.b));
                pixelColor /= pixelCount;

                pixelColors[i] = pixelColor;
            }

            return pixelColors;
        }

        /// <summary>
        /// Pixelates a texture.
        /// </summary>
        /// <param name="texture">The texture that needs to be turned into pixels.</param>
        /// <param name="pixelCount">The total amount of pixels used.</param>
        /// <returns>A pixelated version of the given texture.</returns>
        public static Texture2D Pixelate(Texture2D texture, int pixelCount)
        {
            if (pixelCount < 1)
                return texture;

            int width = pixelCount;
            int height = pixelCount;
            if (texture.width > texture.height)
                height = (int)(pixelCount * ((float)texture.height / (float)texture.width));
            else
                width = (int)(pixelCount * ((float)texture.width / (float)texture.height));

            Texture2D resizedPixels = ResizeHandler.Resize(texture, new Vector2Int(width, height));

            return resizedPixels;
        }

        /// <summary>
        /// Pixelates a texture.
        /// </summary>
        /// <param name="texture">The texture that needs to be turned into pixels.</param>
        /// <param name="newSize">The new size the pixelated image will have.</param>
        /// <param name="pixelCount">The total amount of pixels used.</param>
        /// <returns>A pixelated version of the given texture.</returns>
        public static Texture2D Pixelate(Texture2D texture, Vector2Int newSize, int pixelCount)
        {
            Texture2D newTexture = Pixelate(texture, pixelCount);
            Texture2D resizedPixels = ResizeHandler.Resize(newTexture, newSize);

            return resizedPixels;
        }
    }
}