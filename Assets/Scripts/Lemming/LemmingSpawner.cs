using UnityEngine;
using TMPro;
using System;

public class LemmingSpawner : MonoBehaviour
{
    public static event Action OnAllLemmingsSpawned;

    [SerializeField] Lemming _lemmingPrefab;
    [SerializeField] int _totalLemmings = 25;
    [SerializeField] float _spawnDelay = 1.3f;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Transform _spawnPoint;

    int _spawnCount = 0;
    float _timer = 0;
    bool _allSpawned, _levelStarted;

    void OnEnable()
    {
        LevelManager.OnLevelStarted += LevelManager_OnLevelStarted;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= LevelManager_OnLevelStarted;
    }

    void Start()
    {
        _text.text = $"{_totalLemmings - _spawnCount}";
    }

    void Update()
    {
        if(_allSpawned || !_levelStarted) { return; }

        _timer += Time.deltaTime;

        if(_timer >= _spawnDelay)
        {
            _timer = 0;
            Lemming newLemming = Instantiate(_lemmingPrefab, _spawnPoint.position, Quaternion.identity, transform);
            newLemming.name = $"Lemming {_spawnCount}";
            _spawnCount++;
            _text.text = $"{_totalLemmings - _spawnCount}";
            if(_spawnCount >= _totalLemmings)
            {
                OnAllLemmingsSpawned?.Invoke();
                _allSpawned = true;
            }
        }
    }

    void LevelManager_OnLevelStarted()
    {
        _levelStarted = true;
    }
}
