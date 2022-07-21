using AudioManagment;
using Entities;
using UnityEngine;

#pragma warning disable CS0649

namespace Spawners
{
    public class BulletsSpawner : MonoBehaviourWithEvents
    {
        [SerializeField]
        private BulletBehaviour _bulletPrefab;
        [SerializeField]
        private int _initialPoolSize = 64;

        private CollisionSystem _collisionSystem;
        private AudioManager _audioManager;

        [HideInInspector]
        public GameobjectsPool<BulletBehaviour> BulletsPool;
        
        [SerializeField]
        private float _bulletsSpeed = 5;
        
        protected override void Awake()
        {
            base.Awake();
            
            BulletsPool = new GameobjectsPool<BulletBehaviour>(_bulletPrefab, _initialPoolSize);
        }

        private void Start()
        {
            _collisionSystem = DependencyContainer.Instance.CollisionSystem;
            _audioManager = DependencyContainer.Instance.AudioManager;
            
            InitializeEvents();
        }

        public void SpawnBullet(Vector3 position, Vector3 direction, Color color, bool playersBullet)
        {
            BulletsPool.GetFromPool()
                .Pool(position, direction, _bulletsSpeed, color, playersBullet);
            
            _audioManager.Play(SoundType.Shoot);
        }

        void RemoveBullet(BulletBehaviour b) => BulletsPool.BackToPool(b);

        protected override void OnGameRestart()
        {
            BulletsPool.BackAllEntitiesToPool();
        }

        void InitializeEvents()
        {
            _collisionSystem.OnBulletHitAsteroid += BulletHitAsteroidHandler;
            _collisionSystem.OnBulletHitPlayer += BulletHitPlayerHandler;
            _collisionSystem.OnBulletHitUfo += BulletHitUfoHandler;
        }

        private void BulletHitUfoHandler(BulletBehaviour bullet)
        {
            RemoveBullet(bullet);
            _audioManager.Play(SoundType.LargeHit);
        }

        private void BulletHitPlayerHandler(BulletBehaviour bullet)
        {
            RemoveBullet(bullet);
            _audioManager.Play(SoundType.LargeHit);
        }

        private void BulletHitAsteroidHandler(BulletBehaviour bullet, AsteroidBehaviour asteroid)
        {
            RemoveBullet(bullet);
        }
    }
}