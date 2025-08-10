using UnityEngine;
using System;

public class Paddle : MonoBehaviour
{
    public static event Action<float> OnPaddleLaunch;
    public static event Action OnPaddleDestroyed;

    [SerializeField] float _moveSpeed = 10f, _launchForce = 15f;
    [SerializeField] float _minX = -8.35f, _maxX = 8.35f;

    float _xForce = 0;

    void Update()
    {
        Launch();
        MoveHorizontal();
    }

    void Launch()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnPaddleLaunch?.Invoke(_xForce);
        }
    }

    void MoveHorizontal()
    {
        _xForce = 0;

        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.left);
            if(transform.position.x < _minX)
            {
                transform.position = new(_minX, transform.position.y, transform.position.z);
            }
            _xForce = -_launchForce;
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
            if(transform.position.x > _maxX)
            {
                transform.position = new(_maxX, transform.position.y, transform.position.z);
            }
            _xForce = _launchForce;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("DEATH!");
            OnPaddleDestroyed?.Invoke();
        }
    }

}
