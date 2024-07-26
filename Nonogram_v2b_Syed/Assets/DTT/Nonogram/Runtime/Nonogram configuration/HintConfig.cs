using System;

namespace DTT.Nonogram
{
    /// <summary>
    /// Holds the hints settings of a Nonogram configuration.
    /// </summary>
    [Serializable]
    public class HintConfig
    {
        /// <summary>
        /// Enables hints.
        /// </summary>
        public bool useHints = true;

        /// <summary>
        /// Sets the amount of hints that may be used.
        /// </summary>
        public int hintAmount = 5;
    }

}
