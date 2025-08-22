using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static event Action OnTransitionInComplete;
    public static event Action OnTransitionOutComplete;

    public TextMeshProUGUI TimerText;

    [SerializeField] RectTransform _maskImage;
    [SerializeField] TextMeshProUGUI _gameOverText, _restartText, _lemmingTotalText, _lemmingKilledText, _lemmingEscapedText, _readyText, _totalScoreText, _introText, _currentScoreText, _pauseText;
    [SerializeField] string _lemmingName;
    [SerializeField] string[] _gameOverMessages;

    int _actvieLemmings = 0, _lostLemmings = 0, _savedLemmings = 0;

    void Awake()
    {
        _lemmingTotalText.text = _lemmingName + " - " + _actvieLemmings.ToString("00");
        _lemmingKilledText.text = "Lost - " + _lostLemmings.ToString("00");
        _lemmingEscapedText.text = "Saved - " + _savedLemmings.ToString("00");
    }

    void Start()
    {
        StartCoroutine(TransitionIn());
    }

    void OnEnable()
    {
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        Lemming.OnAnyLemmingSpawned += Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled += Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
        LevelManager.OnLevelFailed += LevelManager_OnLevelFailed;
        LevelManager.OnLevelWon += LevelManager_OnLevelWon;
        LevelManager.OnLevelStarted += LevelManager_OnLevelStarted;
        LevelManager.OnRestartLevel += LevelManager_OnRestartLevel;
        LevelManager.OnGamePaused += LevelManager_OnGamePaused;
        LevelManager.OnGameUnpaused += LevelManager_OnGameUnpaused;
        ScoreKeeper.OnScoreDisplayed += ScoreKeeper_OnScoreDisplayed;
    }

    void OnDisable()
    {
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        Lemming.OnAnyLemmingSpawned -= Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled -= Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
        LevelManager.OnLevelFailed -= LevelManager_OnLevelFailed;
        LevelManager.OnLevelWon -= LevelManager_OnLevelWon;
        LevelManager.OnLevelStarted -= LevelManager_OnLevelStarted;
        LevelManager.OnRestartLevel -= LevelManager_OnRestartLevel;
        LevelManager.OnGamePaused -= LevelManager_OnGamePaused;
        LevelManager.OnGameUnpaused -= LevelManager_OnGameUnpaused;
        ScoreKeeper.OnScoreDisplayed -= ScoreKeeper_OnScoreDisplayed;
    }

    void Player_OnPlayerKilled()
    {
        _gameOverText.text = _gameOverMessages[UnityEngine.Random.Range(0, _gameOverMessages.Length)];
        _gameOverText.enabled = true;
        _restartText.enabled = true;
    }

    void Lemming_OnAnyLemmingSpawned()
    {
        _actvieLemmings++;
        _lemmingTotalText.text = _lemmingName + " - " + _actvieLemmings.ToString("00");
    }

    void Lemming_OnAnyLemmingKilled()
    {
        _actvieLemmings--;
        _lostLemmings++;
        _lemmingTotalText.text = _lemmingName + " - " + _actvieLemmings.ToString("00");
        _lemmingKilledText.text = "Lost - " + _lostLemmings.ToString("00");
    }

    void Lemming_OnAnyLemmingEscaped()
    {
        _actvieLemmings--;
        _savedLemmings++;
        _lemmingTotalText.text = _lemmingName + " - " + _actvieLemmings.ToString("00");
        _lemmingEscapedText.text = "Saved - " + _savedLemmings.ToString("00");
    }

    void LevelManager_OnLevelFailed()
    {
        _gameOverText.text = $"All {_lemmingName} Lost!";
        _gameOverText.enabled = true;
        _restartText.enabled = true;
    }

    void LevelManager_OnLevelWon()
    {
        _gameOverText.text = "SUCCESS!";
        _restartText.text = "PRESS N FOR NEXT LEVEL\nPRESS R TO RETRY";
        _gameOverText.enabled = true;
        _restartText.enabled = true;
    }

    void LevelManager_OnLevelStarted()
    {
        _readyText.enabled = false;
    }

    void LevelManager_OnRestartLevel()
    {
        StartCoroutine(TransitionOut());
    }

    void LevelManager_OnGamePaused()
    {
        _pauseText.enabled = true;
    }

    void LevelManager_OnGameUnpaused()
    {
        _pauseText.enabled = false;
    }

    void ScoreKeeper_OnScoreDisplayed(int score, int totalSaves)
    {
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _totalScoreText.text = $"SCORE - {score:0000000}\nTOTAL {_lemmingName} SAVED - {totalSaves:000}";
        _totalScoreText.enabled = true;
        StartCoroutine(TransitionOut());
    }

    IEnumerator TransitionOut() // Yes I could make a single coroutine inputting startsize and targetsize but this is FINE for a game jam
    {
        _maskImage.gameObject.SetActive(true);
        Vector2 startSize = _maskImage.sizeDelta;
        Vector2 targetSize = Vector2.zero;
        float elapsedTime = 0;
        float lerpDuration = 3.25f;

        while(elapsedTime < lerpDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            _maskImage.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
            yield return null;
        }

        _maskImage.sizeDelta = targetSize;

        OnTransitionOutComplete?.Invoke();
    }

    IEnumerator TransitionIn()
    {
        Vector2 startSize = _maskImage.sizeDelta;
        Vector2 targetSize = new(2200, 2200);
        float elapsedTime = 0;
        float lerpDuration = 3.25f;

        while(elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            _maskImage.sizeDelta = Vector2.Lerp(startSize, targetSize, t);
            yield return null;
        }

        _maskImage.sizeDelta = targetSize;
        _maskImage.gameObject.SetActive(false);
        _introText.enabled = false;
        OnTransitionInComplete?.Invoke();
        _readyText.enabled = true;
    }

    public void SetTotalScore(int score)
    {
        _currentScoreText.text = $"SCORE - {score:0000000}";
    }
}
