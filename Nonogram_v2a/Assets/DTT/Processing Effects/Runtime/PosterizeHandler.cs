using System.Linq;
using UnityEngine;

namespace DTT.ProcessingEffects
{
    /// <summary>
    /// Handles posterizing colors.
    /// </summary>
    public class PosterizeHandler
    {
        /// <summary>
        /// Posterizes the given color.
        /// </summary>
        /// <param name="color">The color to posterize.</param>
        /// <param name="level">Controls the amount of posterization.</param>
        /// <returns>The given color posertized.</returns>
        public static Color Posterize(Color color, float level)
        {
            level *= .01f;

            float red = (float)Mathf.Floor(color.r / level) * level;
            float green = (float)Mathf.Floor(color.g / level) * level;
            float blue = (float)Mathf.Floor(color.b / level) * level;

            Color newColor = new Color(Mathf.Clamp(red, 0, 255), Mathf.Clamp(green, 0, 255), Mathf.Clamp(blue, 0, 255), color.a);
            return newColor;
        }

        /// <summary>
        /// Posterizes the given color array.
        /// </summary>
        /// <param name="colors">The color array to posterize.</param>
        /// <param name="level">Controls the amount of posterization.</param>
        /// <returns>The given color array posertized.</returns>
        public static Color[] Posterize(Color[] colors, float level)
        {
            colors = colors.Select(color => Posterize(color, level)).ToArray();

            return colors;
        }

        /// <summary>
        /// Posterizes the given texture.
        /// </summary>
        /// <param name="texture">The texture to posterize.</param>
        /// <param name="level">Controls the amount of posterization.</param>
        /// <returns>The given texture posertized.</returns>
        public static Texture2D Posterize(Texture2D texture, float level)
        {
            Color[] newPixels = Posterize(texture.GetPixels(), level);

            Texture2D newTexture = new Texture2D(texture.width, texture.height);
            newTexture.SetPixels(newPixels);
            newTexture.Apply();

            return newTexture;
        }

        /// <summary>
        /// Posterizes the given texture and resizes it.
        /// </summary>
        /// <param name="texture">The texture to posterize.</param>
        /// <param name="newSize">The texture will be resized to this size</param>
        /// <param name="level">Controls the amount of posterization.</param>
        /// <returns>The given texture posertized and resized.</returns>
        public static Texture2D Posterize(Texture2D texture, Vector2Int newSize, float level)
        {
            Texture2D resizedTexture = ResizeHandler.Resize(texture, new Vector2Int(newSize.x, newSize.y));
            Texture2D newTexture = Posterize(resizedTexture, level);

            return newTexture;
        }
    }
}