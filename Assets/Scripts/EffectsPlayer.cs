using UnityEngine;

public class EffectsPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _pemmingLostSFX, _SnacAppearSFX, _collectableSFX, _playerDeathSFX, _killEnemySFX;

    void OnEnable()
    {
        Lemming.OnAnyLemmingKilled += Lemming_OnAnyLemmingKilled;
        CentipedeSpawner.OnCentipedeSpawned += CentipedeSpawner_OnCentipedeSpawned;
        Collectable.OnGetCollectable += Collectable_OnGetCollectable;
        Player.OnPlayerKilled += Player_OnPlayerKilled;
        EnemyHealth.OnAnyEnemyDestroyed += EnemyHealth_OnAnyEnemyDestroyed;
    }

    void OnDisable()
    {
        Lemming.OnAnyLemmingKilled -= Lemming_OnAnyLemmingKilled;
        CentipedeSpawner.OnCentipedeSpawned -= CentipedeSpawner_OnCentipedeSpawned;
        Collectable.OnGetCollectable -= Collectable_OnGetCollectable;
        Player.OnPlayerKilled -= Player_OnPlayerKilled;
        EnemyHealth.OnAnyEnemyDestroyed -= EnemyHealth_OnAnyEnemyDestroyed;
    }

    void Lemming_OnAnyLemmingKilled()
    {
        _audioSource.PlayOneShot(_pemmingLostSFX);
    }

    void CentipedeSpawner_OnCentipedeSpawned()
    {
        _audioSource.PlayOneShot(_SnacAppearSFX);
    }

    void Collectable_OnGetCollectable(Vector2 _, int __)
    {
        _audioSource.PlayOneShot(_collectableSFX);
    }

    void Player_OnPlayerKilled()
    {
        _audioSource.PlayOneShot(_playerDeathSFX);
    }

    void EnemyHealth_OnAnyEnemyDestroyed(EnemyHealth _)
    {
        _audioSource.PlayOneShot(_killEnemySFX);
    }
}
