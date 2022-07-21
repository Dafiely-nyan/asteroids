using System.Collections;
using Entities;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionHandler : MonoBehaviourWithEvents
    {
        private Player _player;
        private CollisionSystem _collisionSystem;
        private PlayerCollisionAnimation _playerCollisionAnimation;

        public float ColliderRadius = 0.43f;

        [SerializeField]
        private float _unvulnerableLength = 3;

        public Transform Transform { get; private set; }
        public bool Vulnerable { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            _player = GetComponent<Player>();
            _playerCollisionAnimation = GetComponent<PlayerCollisionAnimation>();
            Transform = transform;
            Vulnerable = true;
        }

        private void Start()
        {
            _collisionSystem = DependencyContainer.Instance.CollisionSystem;
            
            InitializeEvents();
        }

        protected override void OnGameRestart()
        {
            SetUnvulnerability();
        }

        void InitializeEvents()
        {
            _collisionSystem.OnAsteroidHitPlayer += AsteroidHitPlayerHandler;
            _collisionSystem.OnBulletHitPlayer += BulletHitPlayerHandler;
        }

        private void BulletHitPlayerHandler(BulletBehaviour bullet) => GettingHitHandler();
        private void AsteroidHitPlayerHandler(AsteroidBehaviour asteroid) => GettingHitHandler();

        void GettingHitHandler()
        {
            _player.GetHit();
            
            SetUnvulnerability();
        }

        void SetUnvulnerability()
        {
            Vulnerable = false;
            _playerCollisionAnimation.Play(_unvulnerableLength);
            
            StartCoroutine(ResetVulnerabilty());
        }

        IEnumerator ResetVulnerabilty()
        {
            float elapsed = 0;
            while (elapsed < _unvulnerableLength)
            {
                if (GameState.NonPausedUpdate)
                    elapsed += Time.deltaTime;
                yield return null;
            }
            
            Vulnerable = true;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, ColliderRadius);
        }
    }
}