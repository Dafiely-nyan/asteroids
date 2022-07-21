using TMPro;
using UnityEngine;
using UnityEngine.UI;

// предупреждение, возникающее из-за отсутствия дефолтных значений у приватных полей
#pragma warning disable CS0649

public class GameUiController : MonoBehaviourWithEvents
{
    private Player.Player _player;
    private ScoringSystem _scoringSystem;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _lifesText;
    [SerializeField] private TextMeshProUGUI _controllersSwitcherText;

    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private Button _continueButton;

    private bool _paused;

    private void Start()
    {
        _player = DependencyContainer.Instance.Player;
        _scoringSystem = DependencyContainer.Instance.ScoringSystem;

        SetDefaultValues();

        InitializeEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameState.Started && !GameState.GameOver)
        {
            GameState.SwitchPauseMode();
            SetContinueButtonInteractable();
            _menuCanvas.gameObject.SetActive(GameState.Paused);
        }
    }

    public void Continue()
    {
        GameState.SetPauseMode(false);
        _menuCanvas.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        _menuCanvas.gameObject.SetActive(false);
        GameState.Restart();
    }

    public void SwitchController()
    {
        _player.SwitchController();
        SetControllerButtonText();
    }

    void SetDefaultValues()
    {
        _scoreText.text = "Score: 0";
        _lifesText.text = $"Lifes: {_player.MaxLifes}";
        SetControllerButtonText();
    }

    void SetContinueButtonInteractable()
    {
        _continueButton.interactable = GameState.Started && !GameState.GameOver;
    }

    void SetControllerButtonText() =>
        _controllersSwitcherText.text = $"Controls: {_player.CurrentController.Description}";

    void InitializeEvents()
    {
        _player.OnLifesChange += PlayerLifesChangeHandler;
        _scoringSystem.OnScoreChange += ScoreChangeHandler;
    }

    private void ScoreChangeHandler()
    {
        _scoreText.text = $"Score: {_scoringSystem.Score}";
    }

    private void PlayerLifesChangeHandler()
    {
        _lifesText.text = $"Lifes: {_player.Lifes}";
    }

    protected override void OnGameRestart()
    {
        SetDefaultValues();
    }

    protected override void OnGameOver()
    {
        SetContinueButtonInteractable();
        _menuCanvas.gameObject.SetActive(true);
    }
}