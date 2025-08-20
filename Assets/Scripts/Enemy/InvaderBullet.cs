using UnityEngine;

public class InvaderBullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Lemming lemming))
        {
            lemming.Kill();
        }
        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
