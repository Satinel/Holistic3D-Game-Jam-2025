using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f, _changeDirectionFrequency = 20f;
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] Vector2[] _directions;

    bool _shouldMove;
    float _timer;

    void OnEnable()
    {
        LevelManager.OnLevelStarted += LevelManager_OnLevelStarted;
        LevelManager.OnLevelFailed += StopMoving;
        LevelManager.OnLevelWon += StopMoving;
        Player.OnPlayerKilled += StopMoving;
    }

    void OnDisable()
    {
        LevelManager.OnLevelStarted -= LevelManager_OnLevelStarted;
        LevelManager.OnLevelFailed -= StopMoving;
        LevelManager.OnLevelWon -= StopMoving;
        Player.OnPlayerKilled -= StopMoving;
    }

    void Start()
    {
        transform.right = _directions[Random.Range(0, _directions.Length)];
    }

    void Update()
    {
        if(!_shouldMove) { return; }

        _timer += Time.deltaTime;

        if(_timer > _changeDirectionFrequency)
        {
            transform.right = _directions[Random.Range(0, _directions.Length)];
            _timer = 0;
        }

        _rigidbody2D.linearVelocity = _moveSpeed * transform.right;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lemming lemming))
        {
            lemming.Kill();
        }
        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    void LevelManager_OnLevelStarted()
    {
        _shouldMove = true;
    }

    void StopMoving()
    {
        _shouldMove = false;
        _rigidbody2D.linearVelocityX = 0;
    }
}
