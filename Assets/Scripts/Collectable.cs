using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<Vector2, int> OnGetCollectable;

    [SerializeField] int _scoreValue = 100;

    void GetCollectable()
    {
        OnGetCollectable?.Invoke(transform.position, _scoreValue);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetCollectable();
        }
    }
}
