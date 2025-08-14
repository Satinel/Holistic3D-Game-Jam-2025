using UnityEngine;

public class CentipedeSpawner : MonoBehaviour
{

    [SerializeField] int _totalSegments;
    [SerializeField] Vector2 _spawnPosition = new();
    [SerializeField] Centipede _centipedePrefab;

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
            newSegment.Setup(_centipedePrefab, _player, _waypointManager, 0);
            _previousSegment = newSegment;
        }
    }
}
