namespace DTT.Nonogram
{
    /// <summary>
    /// Holds the result of the completed level.
    /// </summary>
    public struct NonogramResult
    {
        /// <summary>
        /// Total seconds it took to complete the Nonogram.
        /// </summary>
        public readonly int seconds;

        /// <summary>
        /// The amount of times the Nonogram has been checked.
        /// </summary>
        public readonly int timesChecked;

        /// <summary>
        /// The result of the Nonogram.
        /// </summary>
        /// <param name="seconds">Total seconds it took to complete the Nonogram.</param>
        /// <param name="checks">The amount of times the Nonogram has been checked.</param>
        public NonogramResult(int seconds, int checks)
        {
            this.seconds = seconds;
            timesChecked = checks;
        }
    }
}