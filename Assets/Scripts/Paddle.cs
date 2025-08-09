using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] Transform _ball;
    [SerializeField] float _minX = -8.35f, _maxX = 8.35f;

    void Start()
    {
        _ball.GetComponent<Rigidbody>().useGravity = true;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.left);
            if(transform.position.x < _minX)
            {
                transform.position = new(_minX, transform.position.y, transform.position.z);
            }
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
            if(transform.position.x > _maxX)
            {
                transform.position = new(_maxX, transform.position.y, transform.position.z);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform == _ball)
        {
            _ball.GetComponent<Rigidbody>().useGravity = false;
        }
    }

}
