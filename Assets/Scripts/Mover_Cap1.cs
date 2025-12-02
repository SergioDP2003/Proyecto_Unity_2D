using UnityEngine;

public class Mover_Cap1 : MonoBehaviour
{
    enum Direction { Left = -1, None = 0, Right = 1 };
    Direction currentDirection = Direction.None;

    [Header("Movimiento en Tierra")]
    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    float currentVelocity = 0;
    public float jumpForce;

    [Header("Movimiento en Agua")]
    public float waterAcceleration = 1f;
    public float waterMaxVelocity = 2f;
    public float waterGravity = 0.5f;

    private float normalGravity;
    private bool isInWater = false;

    [Header("Componentes")]
    public Rigidbody2D rb2D;
    Colisiones_Cap1 colisiones;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones_Cap1>();
    }

    void Start()
    {
        normalGravity = rb2D.gravityScale;
    }

    void Update()
    {
        currentDirection = Direction.None;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            currentDirection = Direction.Left;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            currentDirection = Direction.Right;
    }

    private void FixedUpdate()
    {
        currentVelocity = rb2D.linearVelocity.x;

        float currentAcceleration = isInWater ? waterAcceleration : acceleration;
        float currentMaxVelocity = isInWater ? waterMaxVelocity : maxVelocity;

        // Movimiento a la derecha
        if (currentDirection > 0)
        {
            if (currentVelocity < 0)
                currentVelocity += ((currentAcceleration + friction) * Time.deltaTime);
            else if (currentVelocity < currentMaxVelocity)
                currentVelocity += currentAcceleration * Time.deltaTime;

            transform.localScale = new Vector2(1, 1);
        }
        // Movimiento a la izquierda
        else if (currentDirection < 0)
        {
            if (currentVelocity > 0)
                currentVelocity -= ((currentAcceleration + friction) * Time.deltaTime);
            else if (currentVelocity > -currentMaxVelocity)
                currentVelocity -= currentAcceleration * Time.deltaTime;

            transform.localScale = new Vector2(-1, 1);
        }
        // Frenado cuando no se pulsa nada
        else
        {
            if (currentVelocity > 1f)
                currentVelocity -= friction * Time.deltaTime;
            else if (currentVelocity < -1f)
                currentVelocity += friction * Time.deltaTime;
            else
                currentVelocity = 0;
        }

        rb2D.linearVelocity = new Vector2(currentVelocity, rb2D.linearVelocity.y);
    }

    void Jump()
    {
        if (colisiones.Grounded())
        {
            rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    // ----------------------------------------
    // SISTEMA DE AGUA
    // ----------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Entrando en el agua");
            isInWater = true;
            rb2D.gravityScale = waterGravity;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Saliendo del agua");
            isInWater = false;
            rb2D.gravityScale = normalGravity;
        }
    }
}
