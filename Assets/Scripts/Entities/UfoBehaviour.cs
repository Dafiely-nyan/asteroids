using Spawners;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class UfoBehaviour : MonoBehaviour, IPoolable
    {
        public float ColliderRadius { get; private set; }
        public Transform Transform { get; private set; }
        
        private Transform _playerTransform;
        private BulletsSpawner _bulletsSpawner;

        private float _coolDown;
        private Vector3 _moveDirection;
        private float _speed;
        
        public void Initialize()
        {
            Transform = transform;
            ColliderRadius = ColliderRadius = Transform.localScale.x * 0.5f;

            _playerTransform = DependencyContainer.Instance.Player.transform;
            _bulletsSpawner = DependencyContainer.Instance.BulletsSpawner;
        }

        public void Pool(Vector3 position, Vector3 direction)
        {
            _coolDown = Random.Range(2f, 5f);
            Transform.position = position;
            _moveDirection = direction;
            _speed = (Utils.VisibleArea.x * 2) / 10;
            
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!GameState.NonPausedUpdate) return;
            
            _coolDown -= Time.deltaTime;
            
            if (_coolDown <= 0)
            {
                ShootTowardsPlayer();
                _coolDown = Random.Range(2f, 5f);
            }
            
            Move();
        }

        void Move()
        {
            if (!GameState.NonPausedUpdate) return;
            Transform.position += _moveDirection * (_speed * Time.deltaTime);
        }

        void ShootTowardsPlayer()
        {
            var position = Transform.position;
            Vector3 direction = (_playerTransform.position - position).normalized;
            
            _bulletsSpawner.SpawnBullet(position + direction, direction, Color.red, false);
        }
    }
}