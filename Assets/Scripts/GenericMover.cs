using UnityEngine;

public class GenericMover : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.25f, _directionChangeFrequencyMax = 7.5f, _directionChangeFrequencyMin = 3.5f;
    [SerializeField] float _maxX = 18.5625f, _minX = 0.1875f;

    float _timer, _changeFrequency = 5f;
    bool _shouldMove;

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
        if(Random.Range(0, 2) > 0)
        {
            SwitchDirection();
        }
    }

    void Update()
    {
        if(!_shouldMove) { return; }

        transform.Translate(_moveSpeed * Time.deltaTime * transform.right);

        if(transform.position.x < _minX)
        {
            transform.position = new(_minX, transform.position.y);
            SwitchDirection();
        }
        if(transform.position.x > _maxX)
        {
            transform.position = new(_maxX, transform.position.y);
            SwitchDirection();
        }

        _timer += Time.deltaTime;
        if(_timer > _changeFrequency)
        {
            if(Random.Range(0, 2) > 0)
            {
                SwitchDirection();
            }
            _timer = 0;
            _changeFrequency = Random.Range(_directionChangeFrequencyMin, _directionChangeFrequencyMax);
        }
    }

    void SwitchDirection()
    {
        _moveSpeed = -_moveSpeed;
        _timer = 0;
    }

    void LevelManager_OnLevelStarted()
    {
        _shouldMove = true;
    }

    void StopMoving()
    {
        _shouldMove = false;
    }
}
