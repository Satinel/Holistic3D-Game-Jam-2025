using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class EndManager : MonoBehaviour
{
    public static event Action OnRestart;

    [SerializeField] GameObject _pressKeyPrompt;
    [SerializeField] float _delay = 30f;
    [SerializeField] TextMeshProUGUI _scoreText, _savedText;

    ScoreKeeper _scoreKeeper;
    bool _gameStarted, _ready;
    float _timer;

    void Awake()
    {
        _scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
    }

    void Start()
    {
        if(_scoreKeeper)
        {
            _scoreText.text = $"FINAL SCORE - {_scoreKeeper.TotalScore:0000000}";
            _savedText.text = $"PEMMINGS SAVED - {_scoreKeeper.TotalSaved:000}";
        }
    }

    void Update()
    {
        if(!_ready)
        {
            if(Input.anyKeyDown)
            {
                SetReady();
            }

            _timer += Time.deltaTime;

            if(_timer >= _delay)
            {
                SetReady();
            }
            return;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartGame();
        }
    }

    void SetReady()
    {
        _ready = true;
        _pressKeyPrompt.SetActive(true);
    }

    public void StartGame()
    {
        if(_gameStarted) { return; }

        OnRestart?.Invoke();

        _gameStarted = true;

        SceneManager.LoadScene(1);
    }
}

