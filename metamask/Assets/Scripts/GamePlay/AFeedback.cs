using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GamePlay
{
    public abstract class AFeedback : MonoBehaviour
    {
        [SerializeField] protected float duration = 3.0f;
        [SerializeField] protected AnimationCurve curve;

        [SerializeField] public UnityEvent onReachedEnd;
        [SerializeField] public UnityEvent onReachedStart;


        private float _normalizedPosition = 0.0f;
        private float _direction = 0.0f;

        public float NormalizedPosition
        {
            get => _normalizedPosition;
            private set => _normalizedPosition = Mathf.Clamp01(value);
        }

        private void OnDisable()
        {
            Stop();
        }

        public void Play()
        {
            _direction = 1.0f;
        }

        public void Revert()
        {
            _direction = -1.0f;
        }

        public void Stop()
        {
            Pause();
            NormalizedPosition = 0.0f;
        }

        public void Pause()
        {
            _direction = 0.0f;
        }

        protected virtual void Update()
        {
            float newNormalizedValue = NormalizedPosition + (_direction * Time.deltaTime) / duration;
            NormalizedPosition = newNormalizedValue;
            if (newNormalizedValue > 1.0f)
            {
                Pause();
                onReachedEnd.Invoke();
            }
            else if (newNormalizedValue < 0.0f)
            {
                Pause();
                onReachedStart.Invoke();
            }
        }
    }
}