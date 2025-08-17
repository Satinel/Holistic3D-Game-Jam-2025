using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    [SerializeField] TextMeshProUGUI _gameOverText, _lemmingTotalText, _lemmingKilledText, _lemmingEscapedText;
    [SerializeField] string _lemmingName;
    [SerializeField] string[] _gameOverMessages;

    int _actvieLemmings = 0, _lostLemmings = 0, _savedLemmings = 0;

    void Awake()
    {
        _lemmingTotalText.text = _lemmingName + " - " + _actvieLemmings.ToString("00");
        _lemmingKilledText.text = "Lost - " + _lostLemmings.ToString("00");
        _lemmingEscapedText.text = "Saved - " + _savedLemmings.ToString("00");
    }

    void OnEnable()
    {
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        Lemming.OnAnyLemmingSpawned += Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled += Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
    }

    void OnDisable()
    {
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        Lemming.OnAnyLemmingSpawned -= Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled -= Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
    }

    void Player_OnPlayerKilled()
    {
        _gameOverText.text = _gameOverMessages[Random.Range(0, _gameOverMessages.Length)];
        _gameOverText.enabled = true;
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
}
