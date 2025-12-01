using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OvejaPatrulla : MonoBehaviour
{
    public float speed = 1f;
    Rigidbody2D rb2D;
    Collider2D col2D; // Referencia al collider para desactivarlo al morir
    bool estaMuerta = false; // Para que deje de moverse si muere

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
        // Si está muerta, no hacemos nada más
        if (estaMuerta) return;

        // Lógica de rebote (si choca con pared)
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

    // --- AQUÍ ESTÁ LA LÓGICA DE APLASTAR ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Comprobamos si choca con la capa "Player"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 2. Comprobamos si el golpe viene desde ARRIBA
            // Miramos los puntos de contacto. Si la normal en Y es negativa, 
            // significa que el golpe vino de arriba hacia abajo.
            foreach (ContactPoint2D punto in collision.contacts)
            {
                // El valor -0.5f da un margen para que no sea estrictamente vertical
                if (punto.normal.y < -0.5f) 
                {
                    Aplastar();
                    break; // Ya la hemos aplastado, salimos del bucle
                }
            }
        }
    }

    void Aplastar()
    {
        if (estaMuerta) return; // Evitar que muera dos veces
        estaMuerta = true;

        // 1. Detener a la oveja
        speed = 0;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic; // Quitar física para que no caiga

        // 2. Desactivar su collider para que el jugador no siga chocando con el cadáver
        col2D.enabled = false;

        // 3. Aplastar visualmente (Reducir altura a la mitad)
        Vector3 escalaAplastada = transform.localScale;
        escalaAplastada.y = escalaAplastada.y * 0.5f; // Mitad de altura
        // Opcional: Aumentar un poco el ancho para efecto gelatina
        // escalaAplastada.x = escalaAplastada.x * 1.2f; 
        transform.localScale = escalaAplastada;

        Debug.Log("¡Oveja aplastada!");

        // 4. Opcional: Destruir el objeto tras medio segundo
        Destroy(gameObject, 0.5f);
    }
}