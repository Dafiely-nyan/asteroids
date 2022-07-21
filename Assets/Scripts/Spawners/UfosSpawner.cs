using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable CS0649

namespace Spawners
{
    public class UfosSpawner : MonoBehaviourWithEvents
    {
        [SerializeField]
        private UfoBehaviour _ufoPrefab;
        [SerializeField]
        private int _initialPoolSize = 1;

        [HideInInspector]
        public GameobjectsPool<UfoBehaviour> UfoPool;

        public UfoBehaviour Ufo { get => UfoPool.Entities[0]; }

        private CollisionSystem _collisionSystem;

        [SerializeField]
        private float _lowerTimeSpawnBound = 20f;
        [SerializeField]
        private float _upperTimeSpawnBound = 40f;

        private float _ufoCooldown;

        private bool _spawned;

        protected override void Awake()
        {
            base.Awake();
            UfoPool = new GameobjectsPool<UfoBehaviour>(_ufoPrefab, _initialPoolSize);
        }

        private void Start()
        {
            _collisionSystem = DependencyContainer.Instance.CollisionSystem;

            SetCoolDown();
            
            InitializeEvents();
        }

        private void Update()
        {
            if (!GameState.NonPausedUpdate) return;
            if (!_spawned)
                _ufoCooldown -= Time.deltaTime;

            if (_ufoCooldown <= 0)
            {
                SpawnUfoOnTime();
                _spawned = true;
                SetCoolDown();
            }
        }

        protected override void OnGameRestart()
        {
            RemoveUfo();
        }

        void SpawnUfoOnTime()
        {
            float allowedHeight = Utils.VisibleArea.y * 0.8f;
            
            Vector2 yRange = new Vector2(-allowedHeight, allowedHeight);

            float heightPosition = Mathf.Lerp(yRange.x, yRange.y, Random.value);

            float signDirection = Mathf.Sign(Random.Range(-1f, 1f));
            
            Vector3 direction = Vector3.right * signDirection;
            Vector3 spawnPosition = new Vector3(Utils.VisibleArea.x * -signDirection, heightPosition);
            
            UfoPool.GetFromPool().Pool(spawnPosition, direction);
        }
        
        void SetCoolDown() => _ufoCooldown = Random.Range(_lowerTimeSpawnBound, _upperTimeSpawnBound);

        void InitializeEvents()
        {
            _collisionSystem.OnAsteroidHitUfo += AsteroidHitUfoHandler;
            _collisionSystem.OnBulletHitUfo += BulletHitUfoHandler;
        }

        private void BulletHitUfoHandler(BulletBehaviour bullet) => RemoveUfo();
        private void AsteroidHitUfoHandler(AsteroidBehaviour asteroid) => RemoveUfo();

        void RemoveUfo()
        {
            UfoPool.BackAllEntitiesToPool();
            SetCoolDown();
            _spawned = false;
        }
    }
}