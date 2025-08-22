using UnityEngine;
using System;

public class Lemming : MonoBehaviour
{
    public static event Action OnAnyLemmingSpawned;
    public static event Action OnAnyLemmingKilled;
    public static event Action OnAnyLemmingEscaped;

    [SerializeField] float _moveSpeed = 1f, _jumpSpeed = 1.25f;
    [SerializeField] float _fallingCheckThreshold = -0.1f;
    [SerializeField] float _blockedCheckDelay = 0.25f;
    [SerializeField] float _climbDelay = 0.15f;
    [SerializeField] float _jumpMaxDuration = 1f, _jumpMinDuration = 0.25f;
    [SerializeField] float _castCheckDistance = 0.2f;
    [SerializeField] LayerMask _dirtLayer;
    [SerializeField] Vector2 _boxSize = Vector2.one;

    Rigidbody2D _rigidbody2D;
    bool _isFalling, _isClimbing, _isJumping, _isWalking;
    float _blockedTimer = 0, _jumpTimer = 0;
    Vector3 _previousPosition;
    float _defaultGravity = 1;
    Animator _animator;

    static readonly int WALK_HASH = Animator.StringToHash("Penging Walk");
    static readonly int FALL_HASH = Animator.StringToHash("Penging Fall");
    static readonly int CLIMB_HASH = Animator.StringToHash("Penging Climb");
    static readonly int JUMP_HASH = Animator.StringToHash("Penging Jump");

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _previousPosition = transform.position;
        OnAnyLemmingSpawned?.Invoke();
    }

    void Start()
    {
        _defaultGravity = _rigidbody2D.gravityScale;
    }

    void Update()
    {
        if(_isJumping)
        {
            _jumpTimer += Time.deltaTime;
            if((_jumpTimer > _jumpMinDuration && _rigidbody2D.linearVelocityY >= 0) || _jumpTimer > _jumpMaxDuration)
            {
                _isJumping = false;
                _rigidbody2D.gravityScale = _defaultGravity;
                _animator.Play(FALL_HASH);
            }
        }
        if(_isFalling) { return; }

        _blockedTimer += Time.deltaTime;
        if(_blockedTimer >= _blockedCheckDelay)
        {
            _blockedTimer = 0;

            if(transform.position.x == _previousPosition.x)
            {
                if(!_isClimbing)
                {
                    if(Physics2D.BoxCast(transform.position, transform.lossyScale, transform.rotation.z, transform.right, _castCheckDistance, _dirtLayer))
                    {
                        _isClimbing = true;
                        _rigidbody2D.gravityScale = 0;
                        _blockedTimer -= _climbDelay;
                        _animator.Play(CLIMB_HASH);
                    }
                    else
                    {
                        transform.right = -transform.right;
                    }
                }
                else if(transform.position.y == _previousPosition.y)
                {
                    transform.right = -transform.right;
                    _isClimbing = false;
                    _rigidbody2D.gravityScale = _defaultGravity / 2;
                    _isJumping = true;
                    _jumpTimer = 0;
                    _animator.Play(JUMP_HASH);
                }
            }

            _previousPosition = transform.position;
        }
    }

    void FixedUpdate()
    {
        bool fallingLastFrame = _isFalling;
        bool walkingLastFrame = _isWalking;
        _isFalling = _rigidbody2D.linearVelocityY < _fallingCheckThreshold;
        _isWalking = !_isFalling && !_isClimbing && !_isJumping;

        if(_isJumping)
        {
            _rigidbody2D.linearVelocity = new(_jumpSpeed * transform.right.x, _rigidbody2D.linearVelocityY);
        }
        else if(_isFalling)
        {
            _rigidbody2D.linearVelocity = new(0, _rigidbody2D.linearVelocityY);
            if(!fallingLastFrame)
            {
                _animator.Play(FALL_HASH);
            }
        }
        else if(_isClimbing)
        {
            _rigidbody2D.linearVelocity = new(_moveSpeed * transform.right.x, _moveSpeed * transform.up.y);

            if(!Physics2D.BoxCast(transform.position, _boxSize, transform.rotation.z, transform.right, _castCheckDistance, _dirtLayer))
            {
                _isClimbing = false;
                _rigidbody2D.gravityScale = _defaultGravity;
            }            
        }
        else
        {
            _isWalking = true;
            _rigidbody2D.linearVelocity = new(_moveSpeed * transform.right.x, _rigidbody2D.linearVelocityY);
            if(!walkingLastFrame)
            {
                _animator.Play(WALK_HASH);
            }
        }
    }

    public void Rescue()
    {
        OnAnyLemmingEscaped?.Invoke();
        Destroy(gameObject);
    }

    public void Kill()
    {
        OnAnyLemmingKilled?.Invoke();
        Destroy(gameObject);
    }
}
