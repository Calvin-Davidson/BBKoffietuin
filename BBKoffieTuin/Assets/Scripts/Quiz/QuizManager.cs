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
        private readonly List<QuizQuestion> _previousQuestions = new();

        [Space]
        public UnityEvent<QuizQuestion> onNewQuestion = new();
        public UnityEvent onGameComplete = new();

        public List<QuizQuestion> Questions => new(questions);

        private void OnEnable()
        {
            if (startOnEnable) DoNextQuestion();
        }

        private void DoNextQuestion()
        {
            _questionsDone += 1;
            if (_questionsDone > questionsToDo)
            {
                onGameComplete?.Invoke();
            }

            QuizQuestion nextQuestions =
                questions.Where(question => !_previousQuestions.Contains(question)).ToList().Shuffle().First();
            
            _previousQuestions.Add(nextQuestions);
            onNewQuestion?.Invoke(nextQuestions);
        }
    }
}
