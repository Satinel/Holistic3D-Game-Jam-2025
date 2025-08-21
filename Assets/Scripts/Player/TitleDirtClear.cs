using UnityEngine;

public class TitleDirtClear : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _fakeCentipede;
    [SerializeField] Ghost _mover;
    [SerializeField] float _moveIncrease = 0.75f;

    bool _speedIncreased;

    static readonly int WALK_HASH = Animator.StringToHash("Player Move");

    void Start()
    {
        _animator.Play(WALK_HASH);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            _fakeCentipede.SetActive(true);

            if(!_speedIncreased)
            {
                _mover.IncreaseMoveSpeed(_moveIncrease);
            }
        }
    }
}
