using UnityEngine;

public class DespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}
