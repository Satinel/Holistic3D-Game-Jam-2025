using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStarted;
    public static event Action OnLevelWon;
    public static event Action OnLevelFailed;
    public static event Action OnNextLevel;
    public static event Action OnRestartLevel;
    public static event Action OnGamePaused;
    public static event Action OnGameUnpaused;

    bool _sceneIsLoaded, _sceneStarted, _playerIsDead, _allLemmingsSpawned, _levelComplete, _levelFailed, _restarting, _inTransition, _isPaused;
    int _activeLemmings, _savedLemmings;

    void OnEnable()
    {
        GameUI.OnTransitionInComplete += GameUI_OnTransitionInComplete;
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        LemmingSpawner.OnAllLemmingsSpawned += LemmingSpawner_OnAllLemmingsSpawned;
        Lemming.OnAnyLemmingSpawned += Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled += Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
        GameUI.OnTransitionOutComplete += GameUI_OnTransitionOutComplete;
    }

    void OnDisable()
    {
        GameUI.OnTransitionInComplete -= GameUI_OnTransitionInComplete;
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        LemmingSpawner.OnAllLemmingsSpawned -= LemmingSpawner_OnAllLemmingsSpawned;
        Lemming.OnAnyLemmingSpawned -= Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled -= Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
        GameUI.OnTransitionOutComplete -= GameUI_OnTransitionOutComplete;
    }

    void Update()
    {
        if(!_sceneIsLoaded || _inTransition) { return; }

        if(!_sceneStarted)
        {
            if(Input.anyKeyDown)
            {
                _sceneStarted = true;
                OnLevelStarted?.Invoke();
            }
        }

        if(_isPaused)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                _inTransition = true;
                _restarting = true;
                OnRestartLevel?.Invoke();
                return;            
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if(_levelComplete)
        {
            if(Input.GetButtonDown("Submit"))
            {
                _inTransition = true;
                OnNextLevel?.Invoke();
            }
            if(Input.GetButtonDown("Fire1"))
            {
                _inTransition = true;
                _restarting = true;
                OnRestartLevel?.Invoke();
            }
            return;
        }

        if(_playerIsDead || _levelFailed)
        {
            if(_restarting) { return; }

            if(Input.GetButtonDown("Fire1"))
            {
                _inTransition = true;
                _restarting = true;
                OnRestartLevel?.Invoke();
            }

            return;
        }

        if(Input.GetButtonDown("Cancel"))
        {
            if(_sceneStarted)
            {
                if(!_isPaused)
                {
                    _isPaused = true;
                    OnGamePaused?.Invoke();
                    Time.timeScale = 0;
                }
                else
                {
                    _isPaused = false;
                    OnGameUnpaused?.Invoke();
                    Time.timeScale = 1;
                }
            }
        }
    }

    void GameUI_OnTransitionInComplete()
    {
        _sceneIsLoaded = true;
    }

    void Player_OnPlayerKilled()
    {
        _playerIsDead = true;
    }

    void LemmingSpawner_OnAllLemmingsSpawned()
    {
        _allLemmingsSpawned = true;
    }

    void Lemming_OnAnyLemmingSpawned()
    {
        _activeLemmings++;
    }

    void Lemming_OnAnyLemmingKilled()
    {
        _activeLemmings--;

        CheckForLevelComplete();
    }

    void Lemming_OnAnyLemmingEscaped()
    {
        _activeLemmings--;
        _savedLemmings++;

        CheckForLevelComplete();
    }

    void CheckForLevelComplete()
    {
        if(_playerIsDead) { return; }

        if(_allLemmingsSpawned && _activeLemmings <= 0)
        {
            if(_savedLemmings > 0)
            {
                _levelComplete = true;
                OnLevelWon?.Invoke();
            }
            else
            {
                _levelFailed = true;
                OnLevelFailed?.Invoke();
            }
        }
    }

    void GameUI_OnTransitionOutComplete()
    {
        if(_restarting)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if(SceneManager.sceneCountInBuildSettings > nextScene)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            ScoreKeeper scoreKeeper = FindFirstObjectByType<ScoreKeeper>();

            if(scoreKeeper)
            {
                scoreKeeper.transform.parent = transform;
            }

            SceneManager.LoadScene(0);
        }
    }
}
