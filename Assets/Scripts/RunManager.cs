using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    [SerializeField] GameObject _pressKeyPrompt;
    float _delay = 5f;
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
