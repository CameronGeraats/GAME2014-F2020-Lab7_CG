using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumBehaviour : MonoBehaviour
{
    public float runSpeed;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidBody2D;
    public Transform lookAheadPoint;
    public LayerMask layerMask;

    private Vector2 direction;

    private bool isGroundAhead;
    void Start()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
       rigidBody2D = GetComponent<Rigidbody2D>();
       isGroundAhead = false;
        direction = Vector2.left;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookAhead();
        _Move();
    }

    private void _Move()
    {
        if (isGroundAhead)
        {
            rigidBody2D.velocity = rigidBody2D.velocity * 0.95f;
            rigidBody2D.AddForce(direction * runSpeed * Time.deltaTime);
        }
        else
        {
            rigidBody2D.velocity = rigidBody2D.velocity * 0.25f;
            direction = (direction == Vector2.left ? Vector2.right : Vector2.left);
//            spriteRenderer.flipX = (spriteRenderer.flipX == true ? false : true);
            transform.localScale = new Vector2(transform.localScale.x * -1,transform.localScale.y);
        }
    }
    private void _LookAhead()
    {
        isGroundAhead = Physics2D.Linecast(transform.position, lookAheadPoint.position,layerMask);
        Debug.DrawLine(transform.position, lookAheadPoint.position,Color.green);
    }
}
