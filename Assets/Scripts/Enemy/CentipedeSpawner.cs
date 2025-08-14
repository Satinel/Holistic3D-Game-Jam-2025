using UnityEngine;

public class CentipedeSpawner : MonoBehaviour
{

    [SerializeField] int _totalSegments;
    [SerializeField] Vector2 _spawnPosition = new();
    [SerializeField] Centipede _centipedePrefab;
    [SerializeField] float _segmentScale = 1;

    Player _player;
    WaypointManager _waypointManager;
    Centipede _previousSegment;


    void Awake()
    {
        _player = FindFirstObjectByType<Player>();
        _waypointManager = FindFirstObjectByType<WaypointManager>();
    }    

    void Start()
    {
        SpawnCentipede(); // TODO Spawn at a set time tracked in another script like LemmingSpawner
    }

    void SpawnCentipede()
    {
        for(int i = 0; i < _totalSegments; i++)
        {
            Centipede newSegment = Instantiate(_centipedePrefab, new(_spawnPosition.x - i * _segmentScale, _spawnPosition.y), Quaternion.identity, transform);
            newSegment.name = $"Segment {i}";

            if(i == 0)
            {
                newSegment.SetAsHead();
            }
            else
            {
                newSegment.SetLeader(_previousSegment.GetComponent<EnemyHealth>());
                _previousSegment.SetFollower(newSegment);
            }
            newSegment.Setup(_player, _waypointManager);
            _previousSegment = newSegment;
        }
    }
}
