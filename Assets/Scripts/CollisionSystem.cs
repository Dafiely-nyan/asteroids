using System;
using System.Collections.Generic;
using Entities;
using Spawners;
using Player;
using UnityEngine;

public class CollisionSystem : MonoBehaviour
{
    public event Action<BulletBehaviour, AsteroidBehaviour> OnBulletHitAsteroid;
    public event Action<BulletBehaviour> OnBulletHitPlayer;
    public event Action<BulletBehaviour> OnBulletHitUfo;
    public event Action<AsteroidBehaviour> OnAsteroidHitPlayer;
    public event Action<AsteroidBehaviour> OnAsteroidHitUfo;

    private List<BulletBehaviour> _bullets;
    private List<AsteroidBehaviour> _asteroids;
    private UfoBehaviour _ufo;
    private PlayerCollisionHandler _playerCollisionHandler;

    private void Start()
    {
        _bullets = DependencyContainer.Instance.BulletsSpawner.BulletsPool.Entities;
        _asteroids = DependencyContainer.Instance.AsteroidsSpawner.AsteroidsPool.Entities;
        _ufo = DependencyContainer.Instance.UfosSpawner.Ufo;
        _playerCollisionHandler = DependencyContainer.Instance.Player.GetComponent<PlayerCollisionHandler>();
    }

    private void LateUpdate()
    {
        if (!GameState.NonPausedUpdate) return;

        ComputeBulletsCollisions();
        ComputeAsteroidsCollision();
    }

    void ComputeBulletsCollisions()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (!_bullets[i].gameObject.activeSelf) continue;

            BulletToAsteroidsCollision(_bullets[i]);
            BulletToPlayerCollision(_bullets[i]);
            BulletToUfoCollision(_bullets[i]);
        }
    }

    void ComputeAsteroidsCollision()
    {
        for (int i = 0; i < _asteroids.Count; i++)
        {
            if (!_asteroids[i].gameObject.activeSelf) continue;

            AsteroidToPlayerCollision(_asteroids[i]);
            AsteroidToUfoCollision(_asteroids[i]);
        }
    }

    void BulletToAsteroidsCollision(BulletBehaviour b)
    {
        for (int i = 0; i < _asteroids.Count; i++)
        {
            if (!_asteroids[i].gameObject.activeSelf) continue;

            if (Vector3.SqrMagnitude(_asteroids[i].Transform.position - b.Transform.position) <=
                Mathf.Pow(_asteroids[i].ColliderRadius + b.ColliderRadius, 2))
            {
                OnBulletHitAsteroid?.Invoke(b, _asteroids[i]);
            }
        }
    }

    void BulletToPlayerCollision(BulletBehaviour b)
    {
        if (!_playerCollisionHandler.Vulnerable) return;
        if (Vector3.SqrMagnitude(_playerCollisionHandler.Transform.position - b.Transform.position) <=
            Mathf.Pow(_playerCollisionHandler.ColliderRadius + b.ColliderRadius, 2))
        {
            OnBulletHitPlayer?.Invoke(b);
        }
    }

    void BulletToUfoCollision(BulletBehaviour b)
    {
        if (!_ufo.gameObject.activeSelf) return;
        if (Vector3.SqrMagnitude(_ufo.Transform.position - b.Transform.position) <=
            Mathf.Pow(_ufo.ColliderRadius + b.ColliderRadius, 2))
        {
            OnBulletHitUfo?.Invoke(b);
        }
    }

    void AsteroidToPlayerCollision(AsteroidBehaviour a)
    {
        if (!_playerCollisionHandler.Vulnerable) return;
        if (Vector3.SqrMagnitude(_playerCollisionHandler.Transform.position - a.Transform.position) <=
            Mathf.Pow(_playerCollisionHandler.ColliderRadius + a.ColliderRadius, 2))
        {
            OnAsteroidHitPlayer?.Invoke(a);
        }
    }

    void AsteroidToUfoCollision(AsteroidBehaviour a)
    {
        if (!_ufo.gameObject.activeSelf) return;
        if (Vector3.SqrMagnitude(_ufo.Transform.position - a.Transform.position) <=
            Mathf.Pow(_ufo.ColliderRadius + a.ColliderRadius, 2))
        {
            OnAsteroidHitUfo?.Invoke(a);
        }
    }
}