using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad;
    public float jump ;
    public float timeBeetweenJump ;
    public float lastJump;
    public  Rigidbody2D pj;

    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundLayer;
    private bool isGrounded;



    public float normalSpeed = 6f;
    public float normalGravity = 3f;

    public float waterSpeed = 2f;     // Movimiento horizontal más lento
    public float waterGravity = 0.5f; // Caída lenta en agua

    private bool isInWater = false;


    void Start()
    {
       pj = GetComponent<Rigidbody2D>();
       pj.gravityScale = normalGravity;

       velocidad = 5f;
       jump = 10f;
       timeBeetweenJump = 0.5f;
       groundRadius = 0.01f;
    }

    void Update()
    {
        ProcesarMovimiento();
        VerificarSuelo();
    }

    void ProcesarMovimiento()
    {
        float currentSpeed = isInWater ? waterSpeed : normalSpeed;
        float move = Input.GetAxis("Horizontal");
        
        pj.linearVelocity = new Vector2(move * currentSpeed,pj.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("ESPACIO DETECTADO");

        if (Input.GetKey(KeyCode.Space) && lastJump < Time.time-timeBeetweenJump && isGrounded)
        {
            Debug.Log("INTENTO DE SALTO — fuerza: " + jump);
            pj.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            lastJump = Time.time;
        }
    }

    void VerificarSuelo()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 
                                             groundRadius,
                                             groundLayer);

        //Debug.Log("Suelo: " + isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Water"))
    {
        Debug.Log("Entrando en el agua: velocidad reducida");
        isInWater = true;
        pj.gravityScale = waterGravity;  // Caída lenta
    }
}

void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Water"))
    {
        isInWater = false;
        pj.gravityScale = normalGravity; // Vuelve a lo normal
    }
}

}
