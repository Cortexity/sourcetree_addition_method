using UnityEngine;

namespace DTT.ProcessingEffects
{
    /// <summary>
    /// Handles resizing textures.
    /// </summary>
    public class ResizeHandler
    {
        /// <summary>
        /// Resizes the given texture to the given size.
        /// </summary>
        /// <param name="texture">The texture that needs to be resized.</param>
        /// <param name="newSize">The output size.</param>
        /// <returns>The texture resized to the given size.</returns>
        public static Texture2D Resize(Texture2D texture, Vector2Int newSize)
        {
            Color[] newPixels = new Color[newSize.x * newSize.y];
            Color[] oldPixels = texture.GetPixels();

            float scaledSizeX = (float)texture.width / newSize.x;
            float scaledSizeY = (float)texture.height / newSize.y;

            for (int x = 0; x < newSize.x; x++)
            {
                for (int y = 0; y < newSize.y; y++)
                {
                    Color pixelColor = oldPixels[(int)(x * scaledSizeX) + ((int)(y * scaledSizeY) * texture.width)];
                    newPixels[x + y * newSize.x] = pixelColor;
                }
            }

            Texture2D newTexture = new Texture2D(newSize.x, newSize.y);
            newTexture.SetPixels(newPixels);
            newTexture.Apply();

            return newTexture;
        }
    }
}