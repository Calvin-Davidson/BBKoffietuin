using System.Collections;
using System.Collections.Generic;
using Quiz;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuizGameResultRenderer : MonoBehaviour
{
    [SerializeField] private QuizManager quizManager;
    [SerializeField] private Image image;
    [SerializeField, Tooltip("Load from worst to best result sprite")] private Sprite[] facialSprite;
    
    
    private void Awake()
    {
        quizManager.onGameComplete.AddListener(HandleQuizComplete);
    }
    

    private void HandleQuizComplete(int correctAnswers)
    {
        image.sprite = facialSprite[correctAnswers];
    }
}
