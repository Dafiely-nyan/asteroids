using UnityEngine;

namespace Entities
{
    public class AsteroidBehaviour : MonoBehaviour, IPoolable
    {
        private float _speed;
        
        public Transform Transform { get; private set; }
        public float ColliderRadius { get; private set; }
        public Vector3 Direction { get; private set; }
        public int Division { get; private set; }
        
        public void Initialize()
        {
            Transform = transform;
        }
        
        public void Pool(Vector3 position, Vector3 direction, float speed, int division, float scale)
        {
            Transform.position = position;
            Transform.localScale = new Vector3(scale, scale, scale) / division;
            ColliderRadius = Transform.localScale.x * 0.5f;
            Direction = direction;
            _speed = speed;
            Division = division;
            
            gameObject.SetActive(true);
        }
        
        private void Update()
        {
            if (!GameState.NonPausedUpdate) return;
            
            Move();
        }

        void Move()
        {
            Transform.position += Direction * (_speed * Time.deltaTime);
        }
    }
}