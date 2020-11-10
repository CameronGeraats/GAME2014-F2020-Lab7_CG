using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public Transform spawnPoint;

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Move();           
    }
    void _Move()
    {
        if (isGrounded)
        {
            if (joystick.Horizontal > joystickHorizontalSensitivity)
            {
                //right
                if (!isCrouching)
                {
                    m_rigidBody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                    m_animator.SetInteger("AnimState", 1);
                }
                else 
                {
                    m_rigidBody2D.AddForce(Vector2.right * horizontalForce * 0.8f * Time.deltaTime);
                    m_spriteRenderer.flipX = false;
                }
            }
            else if (joystick.Horizontal < -joystickHorizontalSensitivity)
            {
                //left
                if (!isCrouching)
                {
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                    m_animator.SetInteger("AnimState", 1);
                }
                else
                {
                    m_rigidBody2D.AddForce(Vector2.left * horizontalForce * 0.8f * Time.deltaTime);
                    m_spriteRenderer.flipX = true;
                }
            }
            else
            {
                //idle
                if(!isJumping && !isCrouching)
                    m_animator.SetInteger("AnimState", 0);
            }
            
            if ((joystick.Vertical > joystickVerticalSensitivity) && !isJumping)
            {
                //jump
                //m_rigidBody2D.AddForce(Vector2.up * verticalForce * Time.deltaTime);
                m_rigidBody2D.AddForce(Vector2.up * verticalForce);
                isJumping = true;
                m_animator.SetInteger("AnimState", 2);
            }
            else if (joystick.Vertical < -joystickVerticalSensitivity)
            {
                //crouch
                m_animator.SetInteger("AnimState", 3);
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
            isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("DeathPlane"))
            transform.position = spawnPoint.position;
    }
}
