using UnityEngine;

public class Fakeipede : MonoBehaviour
{
    [SerializeField] Animator _headAnimator;

    void Start()
    {
        _headAnimator.SetBool("IsHead", true);
    }
}
