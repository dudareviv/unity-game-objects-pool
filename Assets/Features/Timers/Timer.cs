using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Timers
{
    public class Timer : MonoBehaviour, ITimer
    {
        private const float ComparisonTolerance = .125f;

        public UnityEvent onStarted;

        /// <summary>
        /// Прогресс таймера каждый тик.
        /// </summary>
        public UnityEvent<float> onTicked;

        /// <summary>
        /// Остаток таймера каждый тик.
        /// </summary>
        public UnityEvent<float> onTickedElapsed;

        /// <summary>
        /// Прогресс таймера при паузе.
        /// </summary>
        public UnityEvent<float> onPaused;

        public UnityEvent onResumed;

        public UnityEvent onStopped;

        public UnityEvent onFinished;

        [SerializeField]
        private float duration;

        [SerializeField]
        private float elapsed;

        [SerializeField]
        private float tickRate = 0.125f;

        private Coroutine _coroutine;

        public void Run(float seconds)
        {
            if (_coroutine != null)
            {
                Stop();
            }

            duration = seconds;
            elapsed = duration;

            Run();
        }

        public void Run()
        {
            if (_coroutine != null)
            {
                Debug.LogWarning("Trying to start already started timer.", this);
                return;
            }

            var isStarted = Math.Abs(duration - elapsed) < ComparisonTolerance;

            _coroutine = StartCoroutine(Handle());

            onStarted?.Invoke();

            if (isStarted)
            {
                onStarted?.Invoke();
            }
            else
            {
                onResumed?.Invoke();
            }
        }

        public void Resume()
        {
            if (_coroutine != null)
            {
                Debug.LogWarning("Trying to start already started timer.", this);
                return;
            }

            _coroutine = StartCoroutine(Handle());

            onResumed?.Invoke();
        }

        public void Pause()
        {
            if (_coroutine == null)
            {
                Debug.LogWarning("Trying to pause not running timer.", this);
                return;
            }

            StopCoroutine(_coroutine);
            _coroutine = null;

            onPaused?.Invoke(GetProgress());
        }

        public void Stop()
        {
            duration = 0;
            elapsed = 0;

            if (_coroutine == null)
            {
                Debug.LogWarning("Trying to stop not running timer.", this);
                return;
            }

            StopCoroutine(_coroutine);
            _coroutine = null;

            onStopped?.Invoke();
        }

        private IEnumerator Handle()
        {
            while (elapsed > 0)
            {
                yield return new WaitForSeconds(tickRate);
                elapsed -= tickRate;
                onTicked?.Invoke(GetProgress());
                onTickedElapsed?.Invoke(elapsed);
            }

            onFinished?.Invoke();

            _coroutine = null;
        }

        private float GetProgress()
        {
            return 1 - elapsed / duration;
        }
    }
}