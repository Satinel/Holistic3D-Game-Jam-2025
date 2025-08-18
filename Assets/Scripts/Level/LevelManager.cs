using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelWon;
    public static event Action OnLevelFailed;

    bool _playerIsDead, _allLemmingsSpawned, _levelComplete, _levelFailed;
    int _activeLemmings, _savedLemmings;

    void OnEnable()
    {
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        LemmingSpawner.OnAllLemmingsSpawned += LemmingSpawner_OnAllLemmingsSpawned;
        Lemming.OnAnyLemmingSpawned += Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled += Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped += Lemming_OnAnyLemmingEscaped;
    }

    void OnDisable()
    {
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        LemmingSpawner.OnAllLemmingsSpawned -= LemmingSpawner_OnAllLemmingsSpawned;
        Lemming.OnAnyLemmingSpawned -= Lemming_OnAnyLemmingSpawned;
        Lemming.OnAnyLemmingKilled -= Lemming_OnAnyLemmingKilled;
        Lemming.OnAnyLemmingEscaped -= Lemming_OnAnyLemmingEscaped;
    }

    void Update()
    {
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
