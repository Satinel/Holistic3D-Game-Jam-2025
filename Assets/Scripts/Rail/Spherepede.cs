using UnityEngine;

public class Spherepede : MonoBehaviour
{
    [SerializeField] Transform _followTarget;
    [SerializeField] float _moveSpeed = 10f, _arcSize = 15f;

    void Update()
    {
        if(_followTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _followTarget.position, _moveSpeed * Time.deltaTime);
            return;
        }

        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, 
                                                GetSinPosition(), 
                                                _moveSpeed * Time.deltaTime), Quaternion.LookRotation(GetSinPosition()));
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = new(Random.Range(transform.position.x - 10, transform.position.x + 10), 
                                    Random.Range(transform.position.y - 10, transform.position.y + 10), 
                                    transform.position.z + 1);
        return randomPosition;
    }

    Vector3 GetSinPosition()
    {
        Vector3 sinPosition = new(transform.position.x, Mathf.Sin(Time.time) * _arcSize, transform.position.z + _moveSpeed * Time.deltaTime);
        return sinPosition;
    }
}
