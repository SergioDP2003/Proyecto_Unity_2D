using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Mover_Cap1 : MonoBehaviour
{
    enum Direction { Left = -1, None = 0, Right = 1 };
    Direction currentDirection = Direction.None;
    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    float currentVelocity = 0;
    public float jumpForce;

    public Rigidbody2D rb2D;
    Colisiones_Cap1 colisiones;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones_Cap1>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        currentDirection = Direction.None;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            currentDirection = Direction.Left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = Direction.Right;
        }
    }
    private void FixedUpdate()
    {
        currentVelocity = rb2D.linearVelocity.x;
        if(currentDirection > 0)
        {
            if(currentVelocity < 0)
            {
                currentVelocity += ((acceleration + friction) * Time.deltaTime);
            }
            else if(currentVelocity < maxVelocity)
            {
                currentVelocity += acceleration * Time.deltaTime;
                transform.localScale = new Vector2(1, 1);
            }
        }
        else if(currentDirection < 0)
        {
            if(currentVelocity > 0)
            {
                currentVelocity -= ((acceleration + friction) * Time.deltaTime);
            }
            else if(currentVelocity > -maxVelocity)
            {
                currentVelocity -= acceleration * Time.deltaTime;
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else
        {
            if(currentVelocity > 1f)
            {
                currentVelocity -= friction * Time.deltaTime;
            }
            else if(currentVelocity < -1f)
            {
                currentVelocity += friction * Time.deltaTime;
            }
            else
            {
                currentVelocity = 0;
            }
        }
        Vector2 velocity = new Vector2(currentVelocity, rb2D.linearVelocity.y);
        rb2D.linearVelocity = velocity;
    }

    void Jump()
    {
        if (colisiones.Grounded())
        {
            Vector2 fuerza = new Vector2(0, jumpForce);
            rb2D.AddForce(fuerza, ForceMode2D.Impulse);   
        }
    }
    
}
