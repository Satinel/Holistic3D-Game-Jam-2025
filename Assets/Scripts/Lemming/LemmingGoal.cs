using UnityEngine;

public class LemmingGoal : MonoBehaviour
{
    [field:SerializeField] public Transform ScorePositon { get; private set; }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lemming lemming))
        {
            lemming.Rescue();
        }
    }
}
