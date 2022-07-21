using AudioManagment;
using UnityEngine;

namespace Player
{
    public class KeyboardController : PlayerControllerBehaviour
    {
        public override string Description { get; } = "Keyboard";
        protected override void Move()
        {
            float rotAngle = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) rotAngle += 1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) rotAngle -= 1;

            float angleStep = _rotationSpeed * Time.deltaTime;

            var rotation = Transform.rotation;
            
            rotation = Quaternion.RotateTowards(rotation,
                    Quaternion.Euler(0, 0, rotation.eulerAngles.z + rotAngle), angleStep);
            
            Transform.rotation = rotation;

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
            if ((Input.GetKeyDown(KeyCode.Space)) && _coolDown <= 0)
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
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _accelerating = true;
                return true;
            }

            return false;
        }

        bool EndAccelerating()
        {
            if (!_accelerating) return false;
            if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)
                && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)))
            {
                _accelerating = false;
                return true;
            }

            return false;
        }
    }
}