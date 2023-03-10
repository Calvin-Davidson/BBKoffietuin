using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Quiz
{
    public class QuizRenderer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private List<Button> buttons;
        private QuizManager _quizManager;

        private void Awake()
        {
            _quizManager = FindObjectOfType<QuizManager>();
            _quizManager.onNewQuestion.AddListener(Render);
        }

        /// <summary>
        /// Render's the new quiz data, as we shuffle the buttons we can use slot[0] which is always in a random position.
        /// </summary>
        /// <param name="question">The new question we ask</param>
        public void Render(QuizQuestion question)
        {
            List<QuizQuestion> otherQuestions = _quizManager.Questions.Where(quizQuestion => question != quizQuestion).ToList().Shuffle();
            buttons = buttons.Shuffle();
            
            questionText.text = question.Question;
            buttons[0].GetComponent<Image>().sprite = question.Sprite;
            
            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].GetComponent<Image>().sprite = otherQuestions[i-1].Sprite;
            }
        }
    }
}