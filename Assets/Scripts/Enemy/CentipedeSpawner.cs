using UnityEngine;

public class CentipedeSpawner : MonoBehaviour
{

    [SerializeField] int _totalSegments;
    [SerializeField] Vector2 _spawnPosition = new();
    [SerializeField] Centipede _centipedePrefab;
    [SerializeField] float _spawnDelay = 60;

    Player _player;
    WaypointManager _waypointManager;
    Centipede _previousSegment;
    float _timer = 0;
    bool _hasSpawned = false;


    void Awake()
    {
        _player = FindFirstObjectByType<Player>();
        _waypointManager = FindFirstObjectByType<WaypointManager>();
    }

    void Update()
    {
        if(_hasSpawned) { return; }

        _timer += Time.deltaTime;
        if(_timer >= _spawnDelay)
        {
            _hasSpawned = true;
            SpawnCentipede();
        }
    }

    void SpawnCentipede()
    {
        for(int i = 0; i < _totalSegments; i++)
        {
            Centipede newSegment = Instantiate(_centipedePrefab, transform);
            newSegment.name = $"Segment {i}";

            if(i == 0)
            {
                newSegment.SetAsHead();
                newSegment.transform.position = new(_spawnPosition.x, _spawnPosition.y);
            }
            else
            {
                newSegment.SetLeader(_previousSegment.GetComponent<EnemyHealth>());
                _previousSegment.SetFollower(newSegment);
            }
            newSegment.Setup(_centipedePrefab, _player, _waypointManager, 0, i % 2 == 0);
            _previousSegment = newSegment;
        }
    }
}
