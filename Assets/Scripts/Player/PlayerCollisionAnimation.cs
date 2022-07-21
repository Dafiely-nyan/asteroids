using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionAnimation : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private Coroutine _animationCoroutine;

        [SerializeField]
        private int _blinkRate = 2;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play(float length)
        {
            if (_animationCoroutine != null) Stop();
            _animationCoroutine = StartCoroutine(Animation(length));
        }

        void Stop()
        {
            StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        IEnumerator Animation(float length)
        {
            float t = 0;
            while (t < length)
            {
                if (t % (1f / _blinkRate) < 1f / (_blinkRate * 2))
                    _spriteRenderer.color = Color.clear;
                else _spriteRenderer.color = Color.white;
                
                if (GameState.NonPausedUpdate)
                    t += Time.deltaTime;

                yield return null;
            }
            
            _spriteRenderer.color = Color.white;
        }
    }
}