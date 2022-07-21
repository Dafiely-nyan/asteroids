using AudioManagment;
using Spawners;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class DependencyContainer : MonoBehaviour
{
    public Camera MainCamera;
    public CollisionSystem CollisionSystem;
    public Player.Player Player;
    public BulletsSpawner BulletsSpawner;
    public AsteroidsSpawner AsteroidsSpawner;
    public UfosSpawner UfosSpawner;
    public AudioManager AudioManager;
    public ScoringSystem ScoringSystem;

    public static DependencyContainer Instance { get; private set; }
    void Awake() => Instance = this;
}