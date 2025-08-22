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
    [SerializeField] GameObject[] _savedPemmings;

    ScoreKeeper _scoreKeeper;
    bool _gameStarted, _ready;
    float _timer;

    static readonly int JUMP_HASH = Animator.StringToHash("Jump");

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

        foreach(GameObject pemming in _savedPemmings)
        {
            pemming.GetComponent<Animator>().Play(JUMP_HASH, 0, UnityEngine.Random.Range(0f, 0.9f));
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
        if(Input.GetButtonDown("Fire1"))
        {
            RestartGame();
        }
#if !UNITY_WEBGL
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    void SetReady()
    {
        _ready = true;
        _pressKeyPrompt.SetActive(true);
    }

    public void RestartGame()
    {
        if(_gameStarted) { return; }

        OnRestart?.Invoke();

        _gameStarted = true;

        SceneManager.LoadScene(1);
    }
}

