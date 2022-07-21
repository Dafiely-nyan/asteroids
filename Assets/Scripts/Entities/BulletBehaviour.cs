using UnityEngine;

namespace Entities
{
    public class BulletBehaviour : MonoBehaviour, IPoolable
    {
        private Vector3 _direction;
        private float _speed;
        private SpriteRenderer _spriteRenderer;

        public Transform Transform { get; private set; }
        public float ColliderRadius { get; private set; }
        public bool PlayersBullet { get; private set; }

        public void Initialize()
        {
            Transform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Pool(Vector3 position, Vector3 direction, float speed, Color color, bool playersBullet)
        {
            Transform.position = position;
            PlayersBullet = playersBullet;
            ColliderRadius = Transform.localScale.x * 0.5f;
            _direction = direction;
            _speed = speed;
            _spriteRenderer.color = color;
            
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!GameState.NonPausedUpdate) return;
            Move();
        }

        void Move()
        {
            Transform.position += _direction * (_speed * Time.deltaTime);
        }

        // только для эдитора и только для визуализации коллайдера
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.5f * transform.localScale.x);
        }
    }
}