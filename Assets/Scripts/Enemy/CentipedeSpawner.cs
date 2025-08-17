using UnityEngine;

public class CentipedeSpawner : MonoBehaviour
{

    [SerializeField] int _totalSegments;
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
            Centipede newSegment = Instantiate(_centipedePrefab, transform.position, transform.rotation, transform);
            newSegment.name = $"Segment {i}";

            if(i == 0)
            {
                newSegment.SetAsHead();
                newSegment.transform.position = transform.position;
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

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Centipede centipede))
        {
            centipede.ShowFollower();
        }
    }
}
