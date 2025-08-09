using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f, _rowDistance = 0.5f;
    [SerializeField] float _xMax = 10f, _xMin = -10f;

    void OnEnable()
    {
        Paddle.OnPaddleDestroyed += Paddle_OnPaddleDestroyed;
    }

    void OnDisable()
    {
        Paddle.OnPaddleDestroyed -= Paddle_OnPaddleDestroyed;
    }

    void Paddle_OnPaddleDestroyed()
    {
        _moveSpeed = 0;
    }

    void Update()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        if(transform.position.x >= _xMax && _moveSpeed > 0)
        {
            DropDownOneRow();
        }
        if(transform.position.x <= _xMin && _moveSpeed < 0)
        {
            DropDownOneRow();
        }
    }

    void DropDownOneRow()
    {
        transform.position = new(transform.position.x, transform.position.y - _rowDistance, transform.position.z);
        _moveSpeed = -_moveSpeed;
    }
}
