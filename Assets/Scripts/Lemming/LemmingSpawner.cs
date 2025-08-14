using UnityEngine;
using TMPro;

public class LemmingSpawner : MonoBehaviour
{
    [SerializeField] Lemming _lemmingPrefab;
    [SerializeField] int _totalLemmings = 25;
    [SerializeField] float _spawnDelay = 1.3f;
    [SerializeField] TextMeshProUGUI _text;

    int _spawnCount = 0;
    float _timer = 0;

    void Start()
    {
        _text.text = $"{_totalLemmings - _spawnCount}";
    }

    void Update()
    {
        if(_spawnCount >= _totalLemmings) { return; }

        _timer += Time.deltaTime;

        if(_timer >= _spawnDelay)
        {
            _timer = 0;
            Lemming newLemming = Instantiate(_lemmingPrefab, transform.position, Quaternion.identity, transform);
            newLemming.name = $"Lemming {_spawnCount}";
            _spawnCount++;
            _text.text = $"{_totalLemmings - _spawnCount}";
        }
    }
}
