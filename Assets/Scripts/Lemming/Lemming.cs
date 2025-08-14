using UnityEngine;

public class Lemming : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f, _jumpSpeed = 1.25f;
    [SerializeField] float _fallingCheckThreshold = -0.1f;
    [SerializeField] float _blockedCheckDelay = 0.25f;
    [SerializeField] float _climbDelay = 0.15f;
    [SerializeField] float _jumpDuration = 1f;
    [SerializeField] float _castCheckDistance = 0.2f;
    [SerializeField] LayerMask _dirtLayer;
    [SerializeField] Vector2 _boxSize = Vector2.one;

    Rigidbody2D _rigidbody2D;
    bool _isFalling, _isClimbing, _isJumping;
    float _blockedTimer = 0, _jumpTimer = 0;
    Vector3 _previousPosition;
    float _defaultGravity = 1;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _previousPosition = transform.position;
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
            if(_jumpTimer > _jumpDuration)
            {
                _isJumping = false;
                _rigidbody2D.gravityScale = _defaultGravity;
            }
        }
        if(_isFalling) { return; }
        // DrawBox(transform.position + transform.right.normalized * _castCheckDistance, _boxSize, transform.rotation.z, Color.magenta);

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
                }
            }

            _previousPosition = transform.position;
        }
    }

    void FixedUpdate()
    {
        _isFalling = _rigidbody2D.linearVelocityY < _fallingCheckThreshold;

        if(_isJumping)
        {
            _rigidbody2D.linearVelocity = new(_jumpSpeed * transform.right.x, _rigidbody2D.linearVelocityY);
        }
        else if(_isFalling)
        {
            _rigidbody2D.linearVelocity = new(0, _rigidbody2D.linearVelocityY);
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
            _rigidbody2D.linearVelocity = new(_moveSpeed * transform.right.x, _rigidbody2D.linearVelocityY);
        }
    }

    void DrawBox(Vector3 center, Vector2 size, float angle, Color color)
    {
        Vector2 halfSize = size / 2f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Vector2[] corners = new Vector2[4];
        corners[0] = (Vector3)center + rotation * new Vector2(-halfSize.x, -halfSize.y); // Bottom Left
        corners[1] = (Vector3)center + rotation * new Vector2(-halfSize.x, halfSize.y);  // Top Left
        corners[2] = (Vector3)center + rotation * new Vector2(halfSize.x, halfSize.y);   // Top Right
        corners[3] = (Vector3)center + rotation * new Vector2(halfSize.x, -halfSize.y);  // Bottom Right

        Debug.DrawLine(corners[0], corners[1], color);
        Debug.DrawLine(corners[1], corners[2], color);
        Debug.DrawLine(corners[2], corners[3], color);
        Debug.DrawLine(corners[3], corners[0], color);
    }
}
