using System.Linq;
using Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Quiz
{
    public class QuizCompleteAudioPlayer : MonoBehaviour
    {
        public AnswerClip[] Clips;
        public QuizManager QuizManager;
        
        private AnswerClip _clipPlaying;

        public UnityEvent onComplete = new();

        private void Awake()
        {
            AudioManager.Instance.onClipComplete.AddListener(HandleClipComplete);
            QuizManager.onGameComplete.AddListener(HandleQuizComplete);
        }
        
        private void HandleClipComplete(AudioClip audioClip)
        {
            if (audioClip == _clipPlaying.Clip) onComplete?.Invoke();
        }

        private void HandleQuizComplete(int correctAnswers)
        {
            AnswerClip clip = Clips.FirstOrDefault(answerClip => answerClip.AnswersCorrect == correctAnswers);
            _clipPlaying = clip;
            AudioManager.Instance.Play(clip.Clip);
        }
    }
}
