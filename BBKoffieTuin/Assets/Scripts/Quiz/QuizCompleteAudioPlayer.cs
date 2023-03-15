using System.Linq;
using Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Quiz
{
    public class QuizCompleteAudioPlayer : MonoBehaviour
    {
        [System.Serializable]
        public struct AnswerClip
        {
            public int AnswersCorrect;
            public AudioClip clip;
        }

        public AnswerClip[] clips;
        public QuizManager quizManager;
        
        private AnswerClip _clipPlaying;

        public UnityEvent onComplete = new();

        private void Awake()
        {
            AudioManager.Instance.onClipComplete.AddListener(HandleClipComplete);
            quizManager.onGameComplete.AddListener(HandleQuizComplete);
        }
        
        private void HandleClipComplete(AudioClip audioClip)
        {
            if (audioClip == _clipPlaying.clip) onComplete?.Invoke();
        }

        private void HandleQuizComplete(int correctAnswers)
        {
            AnswerClip clip = clips.FirstOrDefault(answerClip => answerClip.AnswersCorrect == correctAnswers);
            _clipPlaying = clip;
            AudioManager.Instance.Play(clip.clip);
        }
    }
}
