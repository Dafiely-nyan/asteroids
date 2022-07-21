using UnityEngine;

public abstract class MonoBehaviourWithEvents : MonoBehaviour
{
    protected virtual void Awake()
    {
        InitializeEventsListeners();
    }

    void InitializeEventsListeners()
    {
        GameState.OnGameOver += OnGameOver;
        GameState.OnPauseChange += OnPauseChange;
        GameState.OnGameRestart += OnGameRestart;
    }

    protected virtual void OnPauseChange(bool paused)
    {
    }

    protected virtual void OnGameOver()
    {
    }

    protected virtual void OnGameRestart()
    {
    }
}