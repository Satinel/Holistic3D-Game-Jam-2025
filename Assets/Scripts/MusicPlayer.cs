using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _failMusic, _winMusic;

    void OnEnable()
    {
        LevelManager.OnLevelStarted += StartMusic;
        LevelManager.OnLevelFailed += HandleFailure;
        LevelManager.OnLevelWon += LevelManager_OnLevelWon;
        LevelManager.OnNextLevel += StopMusic;
        LevelManager.OnRestartLevel += StopMusic;
        RunManager.OnRunStarted += StopMusic;
        EndManager.OnRestart += StopMusic;
        Player.OnPlayerKilled += StopMusic;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= StartMusic;
        LevelManager.OnLevelFailed -= HandleFailure;
        LevelManager.OnLevelWon -= LevelManager_OnLevelWon;
        LevelManager.OnNextLevel -= StopMusic;
        LevelManager.OnRestartLevel -= StopMusic;
        RunManager.OnRunStarted -= StopMusic;
        EndManager.OnRestart -= StopMusic;
        Player.OnPlayerKilled -= HandleFailure;
    }

    void StartMusic()
    {
        _audioSource.Play();
    }

    void HandleFailure()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_failMusic);
        _audioSource.loop = false;
    }

    void LevelManager_OnLevelWon()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(_winMusic);
        _audioSource.loop = false;
    }

    void StopMusic()
    {
        _audioSource.Stop();
    }
}
