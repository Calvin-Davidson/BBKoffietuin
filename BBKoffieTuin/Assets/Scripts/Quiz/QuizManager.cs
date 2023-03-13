using System;
using System.Collections.Generic;
using System.Linq;
using Toolbox.MethodExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace Quiz
{
    public class QuizManager : MonoBehaviour
    {
        [SerializeField] private int questionsToDo = 3;
        [SerializeField] private bool startOnEnable = false;
        [SerializeField] private List<QuizQuestion> questions;

        private int _questionsDone = 0;
        private int _correctAnswerCount = 0;
        private readonly List<QuizQuestion> _previousQuestions = new();
        private QuizQuestion _currentQuestion;

        [Space] public UnityEvent<QuizQuestion> onNewQuestion = new();
        [Tooltip("Invoked when the game end, with the amount of correct answers as value")]
        public UnityEvent<int> onGameComplete = new();

        public List<QuizQuestion> Questions => new(questions);

        public QuizQuestion CurrentQuestion => _currentQuestion;

        private void OnEnable()
        {
            if (startOnEnable) DoNextQuestion();
        }

        private void DoNextQuestion()
        {
            QuizQuestion nextQuestions =
                questions.Where(question => !_previousQuestions.Contains(question)).ToList().Shuffle().First();

            _previousQuestions.Add(nextQuestions);
            onNewQuestion?.Invoke(nextQuestions);
            _currentQuestion = nextQuestions;
        }

        public void HandleAnswerCorrect()
        {
            _questionsDone += 1;
            _correctAnswerCount += 1;
            if (CheckGameComplete()) return;

            DoNextQuestion();
        }

        public void HandleAnswerIncorrect()
        {
            _questionsDone += 1;
            if (CheckGameComplete()) return;

            DoNextQuestion();
        }

        private bool CheckGameComplete()
        {
            if (_questionsDone == questionsToDo)
            {
                onGameComplete?.Invoke(_correctAnswerCount);
                return true;
            }

            return false;
        }
    }
}