using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.ProcessingEffects.Editor
{
    /// <summary>
    /// Manages saving images to the disk.
    /// </summary>
    public class SaveTexture
    {
        /// <summary>
        /// Saves the given texture as the original file type.
        /// </summary>
        /// <param name="texture">The texture to save.</param>
        /// <param name="filePath">The location to save the file.</param>
        /// <param name="fileName">The file name the texture is saved as</param>
        /// <param name="originalTexture">Gets the file extention from this texture, uses .png by default.</param>
        public static void SaveImageToDisk(Texture2D texture, string filePath, string fileName, Texture2D originalTexture)
        {
            byte[] bytes;
            string extension = GetFileExtension(originalTexture);
            switch (extension)
            {
                case ".jpg":
                case ".jepg":
                    bytes = texture.EncodeToJPG();
                    break;
                case ".tga":
                    bytes = texture.EncodeToTGA();
                    break;
                case ".exr":
                    bytes = texture.EncodeToEXR();
                    break;
                default:
                    bytes = texture.EncodeToPNG();
                    break;
            }

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            string fileNameWithExtension = $"{fileName}{extension}";
            filePath = Path.Combine(filePath, fileNameWithExtension);

            File.WriteAllBytes(filePath, bytes);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Gets the file extension from the given texture.
        /// </summary>
        /// <param name="originalTexture">The texture to get the extenison from.</param>
        /// <returns>The extension from the given texture.</returns>
        private static string GetFileExtension(Texture2D originalTexture) => Path.GetExtension(AssetDatabase.GetAssetPath(originalTexture));
    }
}