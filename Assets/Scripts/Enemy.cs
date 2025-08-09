using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _xMax = 10f, _xMin = -10f;
    
    void Update()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        if(transform.position.x >= _xMax)
        {
            transform.position = new(transform.position.x, transform.position.y - 1, transform.position.z);
            _moveSpeed = -_moveSpeed;
        }
        if(transform.position.x <= _xMin)
        {
            transform.position = new(transform.position.x, transform.position.y - 1, transform.position.z);
            _moveSpeed = -_moveSpeed;
        }
    }
}
