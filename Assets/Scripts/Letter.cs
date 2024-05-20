using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Wordle
{
    public class Letter : MonoBehaviour
    {
        TMP_Text tmp;
        Image image;
        private void Awake()
        {
            tmp = GetComponentInChildren<TMP_Text>();
            image = GetComponent<Image>();
            Assert.IsNotNull(tmp);
            Assert.IsNotNull(image);
        }

        private void Start() => ClearLetter();

        public void SetLetter(char letter) => tmp.text = letter.ToString().ToUpper();

        public void ClearLetter() => tmp.text = string.Empty;

        public bool IsEmpty() => tmp.text == string.Empty;

        public void SetColor(Color color) => image.color = color;

        public string GetString() => tmp.text;
    }
}