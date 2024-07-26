using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// The type of generation used.
    /// </summary>
    public enum NonogramGenerationType
    {
        [InspectorName("Image")]
        IMAGE = 0,
        [InspectorName("Random")]
        RANDOM = 1
    }
}
