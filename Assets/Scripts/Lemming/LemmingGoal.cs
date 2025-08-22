using UnityEngine;

public class LemmingGoal : MonoBehaviour
{
    [field:SerializeField] public Transform ScorePositon { get; private set; }

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _rescueSFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lemming lemming))
        {
            lemming.Rescue();
            _audioSource.PlayOneShot(_rescueSFX);
        }
    }
}
