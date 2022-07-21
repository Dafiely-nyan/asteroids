using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Spawners
{
    public class GameobjectsPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _gameobjectPrefab;

        private readonly Queue<T> _pool = new Queue<T>();
        public readonly List<T> Entities = new List<T>();

        public bool IsFull { get => _pool.Count == Entities.Count; }

        public GameobjectsPool(T prefab, int initialPoolSize)
        {
            _gameobjectPrefab = prefab;
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateObjectToPool();
            }
        }

        public T GetFromPool()
        {
            if (_pool.Count > 0)
                return _pool.Dequeue();
            
            CreateObjectToPool();
            return GetFromPool();
        }

        public void BackToPool(T _poolable)
        {
            _poolable.gameObject.SetActive(false);
            
            _pool.Enqueue(_poolable);
        }

        public void BackAllEntitiesToPool()
        {
            foreach (T entity in Entities)
            {
                if (entity.gameObject.activeSelf)
                {
                    entity.gameObject.SetActive(false);
                    _pool.Enqueue(entity);
                }
            }
        }

        void CreateObjectToPool()
        {
            var g = Object.Instantiate(_gameobjectPrefab).GetComponent<T>();
                
            g.Initialize();
            g.gameObject.SetActive(false);

            _pool.Enqueue(g);
            Entities.Add(g);
        }
    }
}