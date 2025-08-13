using UnityEngine;

public class InvaderEnemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.5f;
    [SerializeField] float _speedIncrease = 0.25f;
    [SerializeField] float _rowDistance = 0.5f;
    [SerializeField] float _minX, _maxX, _minY, _maxY;

    Rigidbody2D _rigidbody2D;
    int _verticalDirection = -1;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody2D.linearVelocity = new(_moveSpeed * transform.right.x, 0);

        if(_rigidbody2D.position.x > _maxX)
        {
            _rigidbody2D.position = new(_maxX, _rigidbody2D.position.y);
            RowChange();
        }
        if(_rigidbody2D.position.x < _minX)
        {
            _rigidbody2D.position = new(_minX, _rigidbody2D.position.y);
            RowChange();
        }
    }

    void RowChange()
    {
        transform.right = -transform.right;
        _moveSpeed += _speedIncrease;

        _rigidbody2D.MovePosition(new(_rigidbody2D.position.x, _rigidbody2D.position.y + (_rowDistance * _verticalDirection)));
        
        if(_rigidbody2D.position.y > _maxY)
        {
            transform.position = new(_rigidbody2D.position.x, _maxY);
            _verticalDirection = -_verticalDirection;
        }
        if(_rigidbody2D.position.y < _minY)
        {
            transform.position = new(_rigidbody2D.position.x, _minY);
            _verticalDirection = -_verticalDirection;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
