using AudioManagment;
using Spawners;
using UnityEngine;

namespace Player
{
    public abstract class PlayerControllerBehaviour : MonoBehaviourWithEvents
    {
        protected Transform Transform { get; private set; }
        
        protected BulletsSpawner _bulletsSpawner;
        protected AudioManager _audioManager;

        [SerializeField]
        protected float _maxSpeed = 3;
        [SerializeField]
        protected float _rotationSpeed = 180;
        [SerializeField]
        protected float _acceleration = 5;
        [SerializeField]
        protected int _shootsPerSecond = 3;
     
        protected Vector3 _speedDirection;
        protected float _coolDown;
        protected bool _accelerating;
        
        public Vector3 SpeedDirection
        {
            get => _speedDirection;
            set
            {
                _speedDirection = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            Transform = transform;
        }
        
        protected virtual void Start()
        {
            _audioManager = DependencyContainer.Instance.AudioManager;
            _bulletsSpawner = DependencyContainer.Instance.BulletsSpawner;
        }

        protected virtual void Update()
        {
            if (!GameState.NonPausedUpdate) return;
            
            ReduceCooldown();
            
            Move();
            Shoot();
        }

        void ReduceCooldown()
        {
            if (_coolDown > 0)
                _coolDown -= Time.deltaTime;
        }

        protected override void OnPauseChange(bool paused)
        {
            if (enabled)
                _audioManager.Stop(SoundType.Acceleration);
        }

        protected override void OnGameRestart()
        {
            if (!enabled) return;
            
            Transform.position = Vector3.zero;
            _speedDirection = Vector3.zero;
            _coolDown = 0;
            _accelerating = false;
        }

        protected override void OnGameOver()
        {
            if (enabled)
                _audioManager.Stop(SoundType.Acceleration);
        }

        public abstract string Description { get; }
        protected abstract void Move();
        protected abstract void Shoot();
    }
}