using AudioManagment;
using UnityEngine;

namespace Player
{
    public class MouseKeyboardController : PlayerControllerBehaviour
    { 
        public override string Description { get; } = "Mouse + Keyboard";
        protected override void Move()
        {
            Vector3 lookDirection = (Utils.CursorWorldPosition - Transform.position).normalized;
            
            float angleStep = _rotationSpeed * Time.deltaTime;

            float targetAngle = Vector2.SignedAngle(Vector2.up, lookDirection);

            Transform.rotation =
                Quaternion.RotateTowards(Transform.rotation, Quaternion.Euler(0, 0, targetAngle), angleStep);
            
            if (StartAccelerating()) _audioManager.Play(SoundType.Acceleration);
            if (_accelerating)
            {
                Vector3 moveDirection = Transform.rotation * Vector3.up;

                _speedDirection += moveDirection * (_acceleration * Time.deltaTime);
                _speedDirection = Vector3.ClampMagnitude(_speedDirection, _maxSpeed);
            }
            if (EndAccelerating()) _audioManager.Stop(SoundType.Acceleration);

            Transform.position += _speedDirection * Time.deltaTime;
        }

        protected override void Shoot()
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && _coolDown <= 0)
            {
                var rotation = Transform.rotation;
                
                Vector3 startPos = Transform.position + rotation * Vector3.up;
                
                _bulletsSpawner.SpawnBullet(startPos, rotation * Vector3.up, Color.green, true);

                _coolDown += (float) 1 / _shootsPerSecond;
            }
        }
        
        bool StartAccelerating()
        {
            if (_accelerating) return false;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                _accelerating = true;
                return true;
            }

            return false;
        }

        bool EndAccelerating()
        {
            if (!_accelerating) return false;
            if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Mouse1))
                && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.Mouse1))
            {
                _accelerating = false;
                return true;
            }

            return false;
        }
    }
}