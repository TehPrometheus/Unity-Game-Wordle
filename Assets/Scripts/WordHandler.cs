using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Wordle
{
    public class WordHandler : MonoBehaviour
    {
        string wordSolution = string.Empty;
        HashSet<string> validWordsSet = new HashSet<string>();
        [SerializeField] TextAsset wordleWordsTxt;
        [SerializeField] TextAsset validWordsTxt;

        private void Start()
        {
            LoadValidWords();
            wordSolution = GenerateRandomWord();
        }

        private void LoadValidWords()
        {
            string validWords = validWordsTxt.text;
            string currentWord = string.Empty;
            int idx = 0;
            int newLineIdx = -1;

            while (idx + 5 < validWords.Length)
            {
                newLineIdx = validWords.IndexOf("\n", idx);
                if (newLineIdx < 0)
                    break;
                currentWord = validWords.Substring(newLineIdx + 1, 5).ToUpper();
                validWordsSet.Add(currentWord);
                idx = newLineIdx + 1;
            }

        }


        private string GenerateRandomWord()
        {
            // Assumption: The txt file contains capitalized 5 letter words separated by \n
            string allWordleWords = wordleWordsTxt.text.ToUpper();
            int randomIndex = Random.Range(0, allWordleWords.Length - 6); // max is -6 to ensure that there's at least one \n to the right of this random index
            int randomNewLineIdx = allWordleWords.IndexOf("\n", randomIndex);
            wordSolution = allWordleWords.Substring(randomNewLineIdx + 1, 5);

            if (!validWordsSet.Contains(wordSolution))
                wordSolution = "APPLE";

            return wordSolution;
        }

        public string GetSolution() => wordSolution;

        public Color[] GetColorState(string guess)
        {
            Assert.IsTrue(guess.Length == wordSolution.Length); // If this fails oh boy

            guess = guess.ToUpper();
            wordSolution = wordSolution.ToUpper();


            int solutionIndex = -1;
            bool isInCorrectSpot = false;
            char currentChar = '0';
            Color[] colors = new Color[wordSolution.Length];

            for (int idx = 0; idx < guess.Length; idx++)
            {
                currentChar = guess[idx];

                solutionIndex = wordSolution.IndexOf(currentChar); // -1 if the current character is NOT in the solution, otherwise return the index in the solution where the match occurs
                if (solutionIndex == -1)
                {
                    colors[idx] = Color.gray;
                    continue;
                }

                isInCorrectSpot = (wordSolution[idx] == guess[idx]); // True if the guessed character is in the correct spot.

                if (isInCorrectSpot)
                {
                    colors[idx] = Color.green;
                }
                else
                {
                    colors[idx] = Color.yellow;
                }
            }

            return colors;
        }
    }
}