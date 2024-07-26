using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// The input setting types used for the Nonogram.
    /// </summary>
    public enum SelectionType
    {
        [InspectorName("Toggle")]
        DEFAULT_TOGGLE,
        [InspectorName("Colors")]
        SELECT_COLOR,
    }
}
