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
    [SerializeField] TextMeshProUGUI _gameOverText, _restartText, _lemmingTotalText, _lemmingKilledText, _lemmingEscapedText, _readyText, _totalScoreText;
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
        _restartText.text = "PRESS ANY KEY";
        _gameOverText.enabled = true;
        _restartText.enabled = true;
    }

    void LevelManager_OnLevelStarted()
    {
        _readyText.enabled = false;
    }

    void ScoreKeeper_OnScoreDisplayed(int score)
    {
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _totalScoreText.text = "SCORE - " + score.ToString("0000000");
        _totalScoreText.enabled = true;
        StartCoroutine(TransitionOut());
    }

    IEnumerator TransitionOut()
    {
        Vector2 startSize = _maskImage.sizeDelta;
        Vector2 targetSize = Vector2.zero;
        float elapsedTime = 0;
        float lerpDuration = 5f;

        while(elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
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
        Vector2 targetSize = new(6400, 6400);
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

        OnTransitionInComplete?.Invoke();
        _readyText.enabled = false;
    }
}
