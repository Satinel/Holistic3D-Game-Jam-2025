using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    bool _gameStarted;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if(_gameStarted) { return; }

        _gameStarted = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
