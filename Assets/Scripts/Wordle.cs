using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Wordle
{
    public class Wordle : MonoBehaviour
    {
        public string solution => wordHandler.GetSolution();
        WordHandler wordHandler;
        InputReader inputReader;
        Vector3 originPosition = new Vector3(685, 250, 0);

        public event Action<bool> gameOver;

        int width = 5;
        int height = 6;

        [SerializeField] Canvas canvas;
        [SerializeField] bool debug = true;
        [SerializeField] Letter letterPrefab;
        [SerializeField] float cellSize = 100f;

        GridSystem2D<GridObject<Letter>> grid;

        GridObject<Letter> activeGridObject;
        HashSet<string> guessList = new HashSet<string>();
        
        bool isLastLetterActive => activeGridObject.x == width - 1;
        bool isFirstLetterActive => activeGridObject.x == 0;
        bool isActiveLetterEmpty => activeGridObject.GetValue().IsEmpty();

        private void Start()
        {
            inputReader = GetComponent<InputReader>();
            wordHandler = GetComponent<WordHandler>();
            InitializeGrid();
            activeGridObject = grid.GetValue(0, height - 1);
            if (debug) activeGridObject.GetValue().SetColor(Color.magenta);
            inputReader.inputLetter += HandleInputLetter;
            inputReader.Back += HandleBackSpace;
            inputReader.Submit += HandleEnter;
        }

        private void HandleBackSpace()
        {
            if (isFirstLetterActive)
                return;

            // TODO: Play SFX;
            if (!isLastLetterActive)
            {
                ActivateLetter(activeGridObject.x - 1, activeGridObject.y);
                activeGridObject.GetValue().ClearLetter();
            }
            else if (isActiveLetterEmpty)
            {
                ActivateLetter(activeGridObject.x - 1, activeGridObject.y);
                activeGridObject.GetValue().ClearLetter();
            }
            else
            {
                activeGridObject.GetValue().ClearLetter();
            }
        }

        private void HandleEnter()
        {
            if (!isLastLetterActive || isActiveLetterEmpty)
                return;

            SubmitGuess();

            SetColors(guessList.Last());

            if(guessList.Last() == wordHandler.GetSolution())
            {
                gameOver?.Invoke(true);
                return;
            }

            if (activeGridObject.y != 0)
                ActivateLetter(0, activeGridObject.y - 1);
            else
            {
                // TODO: End of game reached!
                gameOver?.Invoke(false);
            }
        }
        private void SubmitGuess()
        {
            string word = string.Empty;
            
            // Get all the letters of the current row, create a word string and store it in the hashset
            for(int x = 0; x < width; x++)
            {
                word += grid.GetValue(x,activeGridObject.y).GetValue().GetString();
            }
         
            // TODO: Check if the submitted word is a real word
            
            guessList.Add(word);
        }

        private void SetColors(string word)
        {
            Color[] colors = wordHandler.GetColorState(word);
         
            for (int x = 0; x < width; x++)
            {
                grid.GetValue(x,activeGridObject.y).GetValue().SetColor(colors[x]);
            }
        }

        private void HandleInputLetter(char c)
        {
            // TODO: add SFX
            var letter = activeGridObject.GetValue();
            letter.SetLetter(c);

            if(!isLastLetterActive)
                ActivateLetter(activeGridObject.x + 1, activeGridObject.y);
        }

        private void ActivateLetter(int x,int y)
        {
            if (debug) activeGridObject.GetValue().SetColor(Color.white);
            activeGridObject = grid.GetValue(x, y);
            if (debug) activeGridObject.GetValue().SetColor(Color.magenta);
        }

        private void InitializeGrid()
        {
            grid = GridSystem2D<GridObject<Letter>>.CreateVerticalGrid(width, height, cellSize, originPosition, debug);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    CreateLetter(x,y);
                }
            }
        }

        private void CreateLetter(int x,int y)
        {
            var letter = Instantiate(letterPrefab, grid.GetWorldPositionCenter(x, y), Quaternion.identity, canvas.transform);
            var gridObject = new GridObject<Letter>(grid, x, y);
            gridObject.SetValue(letter);
            grid.SetValue(x,y,gridObject);
        }
    }
}