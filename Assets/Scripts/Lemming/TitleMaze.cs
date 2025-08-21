using UnityEngine;

public class TitleMaze : MonoBehaviour
{
    [SerializeField] Lemming _prefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] int _count = 10;
    [SerializeField] float _delay = 0.5f;

    int _active;
    float _timer;

    void Update()
    {
        if(_active >= _count) { return; }

        _timer += Time.deltaTime;

        if(_timer > _delay)
        {
            Instantiate(_prefab, _spawnPoint.position, Quaternion.identity);
            _timer = 0;
            _active++;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = _spawnPoint.position;
    }
}
