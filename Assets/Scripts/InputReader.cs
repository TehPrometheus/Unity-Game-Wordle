using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace Wordle
{
    public class InputReader: MonoBehaviour 
    {
        PlayerInput playerInput;
        InputAction backAction;
        InputAction submitAction;

        public event Action Submit;
        public event Action Back;
        public event Action<char> inputLetter;
        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            backAction = playerInput.actions["Back"];
            submitAction = playerInput.actions["Submit"];

            backAction.performed += OnBackSpace;
            submitAction.performed += OnSubmit;
        }

        private void Update()
        {
            // Check if any key is pressed
            if (Input.anyKeyDown)
            {
                // Loop through the ASCII values for letters (a-z, A-Z)
                for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
                {
                    // Check if the current key is pressed
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        Debug.Log("Letter " + (char)i + " is pressed");
                        OnInputLetter((char)i);
                        break; // Exit the loop once a letter is found
                    }
                }
            }
        }

        public void OnSubmit(InputAction.CallbackContext context) => Submit?.Invoke();
        public void OnBackSpace(InputAction.CallbackContext context) => Back?.Invoke();
        public void OnInputLetter(char letter) => inputLetter?.Invoke(letter);
        void OnDestroy() => submitAction.performed -= OnSubmit;
    }
}