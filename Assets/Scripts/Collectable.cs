using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<int> OnGetCollectable;

    [SerializeField] int _scoreValue = 100;

    void GetCollectable()
    {
        OnGetCollectable?.Invoke(_scoreValue); // TODO Play sound and have a scorekeeper to actually track getting this
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
