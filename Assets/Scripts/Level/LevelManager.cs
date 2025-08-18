using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStarted;
    public static event Action OnLevelWon;
    public static event Action OnLevelFailed;

    bool _sceneIsLoaded, _sceneStarted, _playerIsDead, _allLemmingsSpawned, _levelComplete, _levelFailed;
    int _activeLemmings, _savedLemmings;

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        LemmingSpawner.OnAllLemmingsSpawned += LemmingSpawner_OnAllLemmingsSpawned;
        Lemming.OnAnyLemmingSpawned += Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled += Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        LemmingSpawner.OnAllLemmingsSpawned -= LemmingSpawner_OnAllLemmingsSpawned;
        Lemming.OnAnyLemmingSpawned -= Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled -= Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
    }

    void Update()
    {
        if(!_sceneStarted && _sceneIsLoaded)
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
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
}
