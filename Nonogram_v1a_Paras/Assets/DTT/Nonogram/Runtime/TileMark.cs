using UnityEngine;

namespace DTT.Nonogram
{ 
    /// <summary>
    /// Enum of the marks possible on a tile
    /// </summary>
    public enum TileMark
    {
        [InspectorName("Empty")]
        EMPTY,
        [InspectorName("Not possible")]
        NOT_POSSIBLE,
        [InspectorName("Wrong-mark")]
        WRONG,
    }
}