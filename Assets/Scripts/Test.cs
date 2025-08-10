using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f, _rowDistance = 0.5f;

    Vector2 _direction = Vector2.right;
    Rigidbody2D _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody2D.linearVelocity = _moveSpeed * _direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            DropDownOneRow();
        }
    }

    void DropDownOneRow()
    {
        _rigidbody2D.MovePosition(new(transform.position.x, transform.position.y - _rowDistance));
        _direction = -_direction;
    }
}
