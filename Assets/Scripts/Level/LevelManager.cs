using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStarted;
    public static event Action OnLevelWon;
    public static event Action OnLevelFailed;
    public static event Action OnNextLevel;

    bool _sceneIsLoaded, _sceneStarted, _playerIsDead, _allLemmingsSpawned, _levelComplete, _levelFailed;
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
        if(!_sceneIsLoaded) { return; }

        if(!_sceneStarted)
        {
            if(Input.anyKeyDown)
            {
                _sceneStarted = true;
                OnLevelStarted?.Invoke();
            }
        }

        if(_levelComplete)
        {
            if(Input.anyKeyDown)
            {
                OnNextLevel?.Invoke();
            }

            return;
        }

        if(_playerIsDead || _levelFailed)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            return;
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
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if(SceneManager.sceneCount > nextScene)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
