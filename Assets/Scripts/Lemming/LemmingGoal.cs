using UnityEngine;
using System;

public class LemmingGoal : MonoBehaviour
{
    public static event Action OnLemmingExit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Lemming>())
        {
            collision.gameObject.SetActive(false);
            OnLemmingExit?.Invoke(); // TODO Increase score, check if all Lemmings accounted for, play sfx/vfx, etc.
        }
    }
}
