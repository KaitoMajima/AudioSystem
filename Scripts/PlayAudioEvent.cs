using UnityEngine;
 
namespace PedroAurelio.AudioSystem
{
    public class PlayAudioEvent : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AudioClipSO clipSO;

        [Header("Play/Stop Settings")]
        [SerializeField] private bool playOnStart;
        [SerializeField] private bool playOnNextEnable;
        [SerializeField] private bool stopOnDisable;

        [Header("Fade Settings")]
        [SerializeField] private float fadeInDuration = 0f;
        [SerializeField] private float fadeOutDuration = 0f;

        [Header("Other Settings")]
        [SerializeField] private Vector2 delayRange = Vector2.zero;

        private bool _hasPlayedOnStart;
        private int _playerID;

        private void OnValidate()
        {
            if (delayRange.x > delayRange.y)
                delayRange.x = delayRange.y;

            if (delayRange.x < 0f)
                delayRange.x = 0f;
            
            if (delayRange.y < delayRange.x)
                delayRange.y = delayRange.x;
        }

        private void Start()
        {
            if (playOnStart)
            {
                PlayAudio();
                _hasPlayedOnStart = true;
            }
        }

        public void PlayAudio()
        {
            var delay = delayRange == Vector2.zero ? 0f : Random.Range(delayRange.x, delayRange.y);
            AudioManager.Instance.RequestAudioPlayer(out _playerID, clipSO, fadeInDuration, transform.position, delay);
        }

        public void StopAudio()
        {
            if (_playerID == -1)
                return;
            
            AudioManager.Instance.StopAudioPlayer(_playerID, fadeOutDuration);
        }

        private void OnEnable()
        {
            if (!_hasPlayedOnStart)
                return;

            if (playOnNextEnable)
                PlayAudio();
        }

        private void OnDisable()
        {
            if (stopOnDisable)
                StopAudio();
        }
    }
}