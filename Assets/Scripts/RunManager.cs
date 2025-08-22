using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public static event Action OnRunStarted;

    [SerializeField] GameObject _pressKeyPrompt;
    [SerializeField] float _delay = 5f;
    bool _gameStarted, _ready;

    float _timer;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.anyKeyDown)
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

        _gameStarted = true;

        OnRunStarted?.Invoke();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
