using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.Nonogram.Demo
{
    /// <summary>
    /// Renders in the text of the numbers and places them in the right places.
    /// </summary>
    public class NumberDisplay : MonoBehaviour
    {
        /// <summary>
        /// The prefab used to display the numbers on the sides of the Nonogram.
        /// </summary>
        [SerializeField] 
        private Text _numberPrefab;

        /// <summary>
        /// Places the numbers inside of the text.
        /// </summary>
        /// <param name="numbers">Array of numbers given for 1 row/column.</param>
        /// <param name="colorOfNumber">Color of the numbers.</param>
        /// <param name="isColumn">Defines if the numbers array is a column or row.</param>
        /// <param name="type">The input type used for the Nonogram.</param>
        /// <return>Return the list of spawned text objects.</return>
        public void RenderNumbers(int[] numbers, Color[] colorOfNumber, bool isColumn, SelectionType type)
        {
            HorizontalOrVerticalLayoutGroup group;
            if (isColumn)
            {
                group = gameObject.AddComponent<VerticalLayoutGroup>();
                group.childAlignment = TextAnchor.LowerCenter;
                group.padding.bottom = -2;
                group.spacing = -5;

                group.reverseArrangement = true;
            }
            else
            {
                group = gameObject.AddComponent<HorizontalLayoutGroup>();
                group.childAlignment = TextAnchor.MiddleRight;
                group.padding.right = 2;
                group.spacing = 5;

                group.reverseArrangement = true;
            }

            SetupLayoutGroup(group);

            List<Text> spawnedTexts = new List<Text>();
            int length = numbers.Length;
            for (int i = 0; i < length; i++)
            {
                Text spawnedText = Instantiate(_numberPrefab, transform);
                spawnedTexts.Add(spawnedText);
                spawnedText.text = numbers[i].ToString();
                if(type == SelectionType.SELECT_COLOR)
                    spawnedText.color = colorOfNumber[i];
                else
                    spawnedText.color = i % 2 == 0 ? colorOfNumber[i] : FuseColors(Color.magenta, colorOfNumber[i]);

                spawnedText.alignment = isColumn ? TextAnchor.LowerCenter : TextAnchor.MiddleRight;
            }
        }

        /// <summary>
        /// Fuses the given colors.
        /// </summary>
        /// <param name="firstColor">The first color.</param>
        /// <param name="secondColor">The second color.</param>
        /// <returns>The avarage color of the 2 given colors.</returns>
        private Color FuseColors(Color firstColor, Color secondColor) => (firstColor + secondColor) / 2;

        /// <summary>
        /// Sets up the layoutgroup. 
        /// </summary>
        /// <param name="group">The layoutgroup to set up.</param>
        private void SetupLayoutGroup(HorizontalOrVerticalLayoutGroup group)
        {
            group.childControlHeight = true;
            group.childControlWidth = true;

            group.childForceExpandHeight = false;
            group.childForceExpandWidth = false;
        }
    }
}