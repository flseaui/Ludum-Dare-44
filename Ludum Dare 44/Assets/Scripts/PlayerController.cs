using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D RB;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    
    private bool _left, _right, _up;

    private bool _grounded;
    
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        
    }

    private void Update()
    {
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _up = Input.GetKey(KeyCode.W);
    }

    private void FixedUpdate()
    {  
        _grounded = Physics2D.BoxCast(transform.position, new Vector3(1, 1f), 0,
            Vector2.down, .1f);
        
        if (_grounded && _up)
        {
            RB.velocity = new Vector2(RB.velocity.x, 0);
            RB.AddForce(new Vector2(0, _jumpHeight), ForceMode2D.Impulse);
        }
        
        if (_left && _right)
        {
            RB.velocity = new Vector2(0, RB.velocity.y);
        }
        else
        {
            if (_left)
            {
                RB.velocity = new Vector2(-_speed * Time.fixedDeltaTime, RB.velocity.y);
            }
            else if (_right)
            {
                RB.velocity = new Vector2(_speed* Time.fixedDeltaTime, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(0, RB.velocity.y);
            }
        }
    }
}
