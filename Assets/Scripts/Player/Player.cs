using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable CS0649

namespace Player
{
    public class Player : MonoBehaviourWithEvents
    {
        public event Action OnLifesChange;
        
        public PlayerControllerBehaviour CurrentController { get => _controllers[_currentControllerIndex]; }
        public int Lifes { get; private set; }
        public int MaxLifes { get => _maxLifes; }
        
        private List<PlayerControllerBehaviour> _controllers;
        private int _currentControllerIndex;
        
        [SerializeField]
        private int _maxLifes = 5;

        protected override void Awake()
        {
            base.Awake();
            
            Lifes = _maxLifes;
            InitializeControllers();
        }

        public void GetHit()
        {
            Lifes -= 1;
            OnLifesChange?.Invoke();
            
            if (Lifes <= 0)
                GameState.SetGameOver();
        }

        public void SwitchController()
        {
            Vector3 currentSpeedDirection = CurrentController.SpeedDirection;
            
            _controllers[_currentControllerIndex].enabled = false;
            _currentControllerIndex = (_currentControllerIndex + 1) % _controllers.Count;
            _controllers[_currentControllerIndex].enabled = true;

            CurrentController.SpeedDirection = currentSpeedDirection;
        }

        void InitializeControllers()
        {
            _controllers = GetComponents<PlayerControllerBehaviour>().ToList();
            
            for (int i = 0; i < _controllers.Count; i++)
            {
                _controllers[i].enabled = false;
            }

            _controllers[_currentControllerIndex].enabled = true;
        }

        protected override void OnGameRestart()
        {
            Lifes = _maxLifes;
            OnLifesChange?.Invoke();
        }
    }
}