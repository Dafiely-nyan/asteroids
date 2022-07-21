using System;

public static class GameState
{
    public static event Action<bool> OnPauseChange;
    public static event Action OnGameOver;
    public static event Action OnGameRestart;

    public static bool Paused { get; private set; }
    public static bool GameOver { get; private set; }
    public static bool Started { get; private set; }
    public static bool NonPausedUpdate { get => !Paused && !GameOver && Started; }

    public static void SetPauseMode(bool paused)
    {
        Paused = paused;
        OnPauseChange?.Invoke(Paused);
    }

    public static void SwitchPauseMode()
    {
        Paused = !Paused;
        OnPauseChange?.Invoke(Paused);
    }

    public static void SetGameOver()
    {
        GameOver = true;
        OnGameOver?.Invoke();
    }

    public static void Restart()
    {
        Started = true;
        Paused = false;
        GameOver = false;
        OnGameRestart?.Invoke();
    }
}