using System.Collections;
using AudioManagment;
using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable CS0649

namespace Spawners
{
    public class AsteroidsSpawner : MonoBehaviourWithEvents
    {
        [SerializeField]
        private AsteroidBehaviour _asteroidPrefab;
        [SerializeField]
        private int _initialPoolSize = 64;

        [HideInInspector]
        public GameobjectsPool<AsteroidBehaviour> AsteroidsPool;

        private CollisionSystem _collisionSystem;
        private AudioManager _audioManager;

        [SerializeField] 
        private float _baseAsteroidSize = 1f;
        [SerializeField]
        private float _asteroidMinSpeed = 1f;
        [SerializeField]
        private float _asteroidMaxSpeed = 2f;
        [SerializeField] 
        private int _startAsteroidsAmount = 2;
        [SerializeField]
        private float _angleSpread = 45;
        [SerializeField]
        private float _delayAfterNoAsteroids = 2f;
        
        private const int _maxAsteroidDivisions = 4;

        private int _currentLevel;
        private Coroutine _asteroidsLoop;

        protected override void Awake()
        {
            base.Awake();
            
            AsteroidsPool = new GameobjectsPool<AsteroidBehaviour>(_asteroidPrefab, _initialPoolSize);
        }

        private void Start()
        {
            _collisionSystem = DependencyContainer.Instance.CollisionSystem;
            _audioManager = DependencyContainer.Instance.AudioManager;
            
            InitializeEvents();
        }

        protected override void OnGameRestart()
        {
            if (_asteroidsLoop != null)
            {
                StopCoroutine(_asteroidsLoop);
            }
            
            AsteroidsPool.BackAllEntitiesToPool();
            
            _currentLevel = 0;
            _asteroidsLoop = StartCoroutine(AsteroidsLoop());
        }

        IEnumerator AsteroidsLoop()
        {
            yield return StartCoroutine(WaitForDelay());
            
            SpawnAsteroids(_currentLevel);
            _currentLevel++;

            while (true)
            {
                yield return new WaitUntil(() => AsteroidsPool.IsFull);
                yield return StartCoroutine(WaitForDelay());
                
                SpawnAsteroids(_currentLevel);
                _currentLevel++;
            }
        }

        IEnumerator WaitForDelay()
        {
            float elapsed = 0;
            while (elapsed < _delayAfterNoAsteroids)
            {
                if (GameState.NonPausedUpdate)
                    elapsed += Time.deltaTime;
                yield return null;
            }
        }
        
        void SpawnAsteroids(int level)
        {
            int amount = level + _startAsteroidsAmount;
            
            Vector2 xRange = new Vector2(-Utils.VisibleArea.x, Utils.VisibleArea.x);
            Vector2 yRange = new Vector2(-Utils.VisibleArea.y, Utils.VisibleArea.y);

            for (int i = 0; i < amount; i++)
            {
                float lerpValueX = Random.value;
                float lerpValueY = Random.value;
                
                Vector3 spawnPosition = new Vector3(Mathf.Lerp(xRange.x, xRange.y, lerpValueX), 
                    Mathf.Lerp(yRange.x, yRange.y, lerpValueY));
                
                Vector3 direction = Quaternion.Euler(0,0, Random.Range(0f, 360f)) * Vector3.up;

                AsteroidsPool.GetFromPool()
                    .Pool(spawnPosition, direction, GetRandomSpeed(), 1, _baseAsteroidSize);
            }
        }

        void InitializeEvents()
        {
            _collisionSystem.OnAsteroidHitUfo += AsteroidHitUfoHandler;
            _collisionSystem.OnAsteroidHitPlayer += AsteroidHitPlayerHandler;
            _collisionSystem.OnBulletHitAsteroid += BulletHitAsteroidHandler;
        }

        private void BulletHitAsteroidHandler(BulletBehaviour bullet, AsteroidBehaviour asteroid)
        {
            if (asteroid.Division == _maxAsteroidDivisions)
            {
                AsteroidsPool.BackToPool(asteroid);
                _audioManager.Play(SoundType.SmallHit);
                return;
            }
            AsteroidsPool.BackToPool(asteroid);
            
            Vector3 currentDirection = asteroid.Direction;
            Vector3 position = asteroid.Transform.position;

            Vector3 dir1 = Quaternion.Euler(0, 0, _angleSpread) * currentDirection;
            Vector3 dir2 = Quaternion.Euler(0, 0, -_angleSpread) * currentDirection;
            
            AsteroidsPool.GetFromPool()
                .Pool(position, dir1, GetRandomSpeed(), asteroid.Division * 2, _baseAsteroidSize);
            
            AsteroidsPool.GetFromPool()
                .Pool(position, dir2, GetRandomSpeed(), asteroid.Division * 2, _baseAsteroidSize);
            
            _audioManager.Play(asteroid.Division == 1 ? SoundType.LargeHit : SoundType.MediumHit);
        }

        float GetRandomSpeed() => Random.Range(_asteroidMinSpeed, _asteroidMaxSpeed);

        private void AsteroidHitPlayerHandler(AsteroidBehaviour asteroid)
        {
            AsteroidsPool.BackToPool(asteroid);
            _audioManager.Play(SoundType.LargeHit);
        }

        private void AsteroidHitUfoHandler(AsteroidBehaviour asteroid)
        {
            AsteroidsPool.BackToPool(asteroid);
            _audioManager.Play(SoundType.LargeHit);
        }
    }
}