using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Quiz
{
    public class QuizRenderer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private List<Button> buttons;
        [SerializeField] private float renderDelay;
        
        
        private QuizManager _quizManager;
        private Button _correctButton;

        public UnityEvent onRenderStarted = new();
        public UnityEvent onRenderComplete = new();
        
        private void Awake()
        {
            _quizManager = FindObjectOfType<QuizManager>();
            _quizManager.onNewQuestion.AddListener(Render);
            
            buttons.ForEach(button => button.onClick.AddListener(() => HandleButtonPress(button)));
            Render(_quizManager.CurrentQuestion);
        }

        /// <summary>
        /// Render's the new quiz data, as we shuffle the buttons we can use slot[0] which is always in a random position.
        /// </summary>
        /// <param name="question">The new question we ask</param>
        private void Render(QuizQuestion question)
        {
            StartCoroutine(RenderWithDelay(question));
        }

        private IEnumerator RenderWithDelay(QuizQuestion question)
        {
            onRenderStarted?.Invoke();
            yield return new WaitForSeconds(renderDelay);
            List<QuizQuestion> otherQuestions = _quizManager.Questions.Where(quizQuestion => question != quizQuestion).ToList().Shuffle();
            buttons = buttons.Shuffle();
            
            questionText.text = question.Question;
            buttons[0].GetComponent<Image>().sprite = question.Sprite;
            _correctButton = buttons[0];
            
            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].GetComponent<Image>().sprite = otherQuestions[i-1].Sprite;
            }
            onRenderComplete?.Invoke();
        }

        private void HandleButtonPress(Button button)
        {
            if (button == _correctButton)
            {
                _quizManager.HandleAnswerCorrect();
            }
            else
            {
                _quizManager.HandleAnswerIncorrect();
            }
        }
    }
}