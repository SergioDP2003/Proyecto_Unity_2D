using UnityEngine;
using System.Collections;

public class OvejaPatrulla : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody2D rb2D;
    Collider2D col2D; 
    bool estaMuerta = false; 

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        col2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        if (estaMuerta) return;

        if (Mathf.Abs(rb2D.linearVelocity.x) < 0.1f)
        {
            speed = -speed;
            Flip();
        }
        
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
    }

    void Flip()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            bool esGolpeDesdeArriba = false;

            foreach (ContactPoint2D punto in collision.contacts)
            {
                if (punto.normal.y < -0.5f) 
                {
                    esGolpeDesdeArriba = true;
                    break; 
                }
            }

            if (esGolpeDesdeArriba)
            {
                // Jugador aplasta a oveja
                Aplastar();
            }
            else
            {
                // Oveja daña al jugador
                // Buscamos el script VidaJugador en Don Quijote
                VidaJugador vidaScript = collision.gameObject.GetComponent<VidaJugador>();
                
                if (vidaScript != null)
                {
                    vidaScript.RecibirDano(); // ¡Quitamos un corazón!
                }
            }
        }
    }

    void Aplastar()
    {
        if (estaMuerta) return; 
        estaMuerta = true;

        speed = 0;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic; 
        col2D.enabled = false;

        Vector3 escalaAplastada = transform.localScale;
        escalaAplastada.y = escalaAplastada.y * 0.5f; 
        transform.localScale = escalaAplastada;
        
        Destroy(gameObject, 0.5f);
    }
}