using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private int _maxJumps;

    private float _gravity;
    private int _jumps;
    
    // Inputs
    private bool _left, _right, _up;

    // States
    [ShowInInspector]
    private bool _grounded, _leftWall, _rightWall, _wallSliding, _wallSlidLastFrame, _wallJumped, _canJumpThisFrame, _awayFromWall;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gravity = _rb.gravityScale;
    }
    
    private void Update()
    {
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _up = Input.GetKey(KeyCode.W);
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        var scale = transform.localScale;
        
        _grounded  = Physics2D.BoxCast(position, scale, 0,Vector2.down,  .1f);
        _leftWall  = Physics2D.BoxCast(position, scale, 0,Vector2.left,  .1f);
        _rightWall = Physics2D.BoxCast(position, scale, 0,Vector2.right, .1f);

        _wallSlidLastFrame = _wallSliding;
        _wallSliding = false;
        
        if (_grounded)
        {
            _jumps = _maxJumps;
        }
        
        if (_canJumpThisFrame && _up)
        {
            if (_wallSliding || _wallSlidLastFrame)
                _wallJumped = true;
            --_jumps;
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(new Vector2(0, _jumpHeight), ForceMode2D.Impulse);
        }
        
        _canJumpThisFrame = _jumps > 0;
        
        var velocity = _rb.velocity;
        
        if (_left && _right)
        {
            _rb.velocity = new Vector2(0, velocity.y);
        }
        else
        {
            if (_left)
            {
                _wallSliding = _leftWall && velocity.y < 0;
                _awayFromWall = !_leftWall;
                _rb.velocity = new Vector2(-_speed * Time.fixedDeltaTime, velocity.y);
            }
            else if (_right)
            {
                _wallSliding = _rightWall && _rb.velocity.y < 0;
                _awayFromWall = !_rightWall;
                _rb.velocity = new Vector2(_speed* Time.fixedDeltaTime, velocity.y);
            }
            else
            {
                _rb.velocity = new Vector2(0, velocity.y);
            }
        }

        if (_wallSliding)
        {
            _rb.gravityScale = _gravity / 4;
            if (_wallSlidLastFrame)
            {
                if (_wallJumped)
                {
                    if (_awayFromWall)
                    {
                        _canJumpThisFrame = true;
                        _wallJumped = false;
                    }
                }
                else
                    _canJumpThisFrame = true;
            }
        }
        else
        {
            _rb.gravityScale = _gravity;
        }

        if (_awayFromWall)
            _wallJumped = false;

    }
}
