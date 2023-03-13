using UnityEngine;

namespace Quiz
{
    [System.Serializable]
    public class QuizQuestion
    {
        [SerializeField] private string question;
        [SerializeField] private Sprite sprite;

        public string Question => question;
        public Sprite Sprite => sprite;
    }
}
