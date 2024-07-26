using UnityEngine;

namespace DTT.Nonogram
{
    /// <summary>
    /// Holds the numbers that are displayed on the rows/columns of the Nonogram.
    /// </summary>
    public struct NumberContainer
    {
        /// <summary>
        /// The numbers of the column/row.
        /// </summary>
        public readonly int[] numbers;

        /// <summary>
        /// The colors that the numbers have.
        /// </summary>
        public readonly Color[] numberColor;

        /// <summary>
        /// Holds the numbers that are displayed on the rows/columns of the Nonogram.
        /// </summary>
        /// <param name="numbers">The numbers on 1 row/column</param>
        /// <param name="numberColor">The colors of the numbers</param>
        public NumberContainer(int[] numbers, Color[] numberColor)
        {
            this.numberColor = numberColor;
            this.numbers = numbers;
        }
    }
}
