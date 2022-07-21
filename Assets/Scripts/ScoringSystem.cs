using System;
using Entities;
using Spawners;
using UnityEngine;

public class ScoringSystem : MonoBehaviourWithEvents
{
    public event Action OnScoreChange;

    public int Score { get; private set; }

    [SerializeField] private int _scoreForLargeAsteroid = 20;
    [SerializeField] private int _scoreForMediumAsteroid = 50;
    [SerializeField] private int _scoreForSmallAsteroid = 100;
    [SerializeField] private int _scoreForUfo = 200;

    [SerializeField] private bool _giveScoreIfNotPlayersBullet = true;
    [SerializeField] private bool _giveScoreWhenAsteroidHitUfo = true;

    private CollisionSystem _collisionSystem;

    private void Start()
    {
        _collisionSystem = DependencyContainer.Instance.CollisionSystem;

        InitializeEvents();
    }

    void InitializeEvents()
    {
        _collisionSystem.OnBulletHitAsteroid += BulletHitAsteroidHandler;
        _collisionSystem.OnBulletHitUfo += BulletHitUfoHandler;
        _collisionSystem.OnAsteroidHitUfo += AsteroidHitUfoHandler;
    }

    private void AsteroidHitUfoHandler(AsteroidBehaviour asteroid)
    {
        if (_giveScoreWhenAsteroidHitUfo)
        {
            Score += _scoreForUfo + GetScoreForAsteroid(asteroid);
        }
    }

    private void BulletHitUfoHandler(BulletBehaviour bullet)
    {
        if (!_giveScoreIfNotPlayersBullet && !bullet.PlayersBullet) return;

        Score += _scoreForUfo;

        OnScoreChange?.Invoke();
    }

    private void BulletHitAsteroidHandler(BulletBehaviour bullet, AsteroidBehaviour asteroid)
    {
        if (!_giveScoreIfNotPlayersBullet && !bullet.PlayersBullet) return;

        Score += GetScoreForAsteroid(asteroid);

        OnScoreChange?.Invoke();
    }

    protected override void OnGameRestart()
    {
        Score = 0;

        OnScoreChange?.Invoke();
    }

    int GetScoreForAsteroid(AsteroidBehaviour asteroid)
    {
        switch (asteroid.Division)
        {
            case 1: return _scoreForLargeAsteroid;
            case 2: return _scoreForMediumAsteroid;
            case 4: return _scoreForSmallAsteroid;
            default: return 0;
        }
    }
}