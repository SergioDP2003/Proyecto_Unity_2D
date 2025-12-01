using UnityEngine;

public class SanchoSeguidor : MonoBehaviour
{
    [Header("Referencias")]
    public Transform objetivo; 
    public Rigidbody2D rb;
    public Animator animator;

    // --- NUEVO: Arrastra aquí el objeto 'Canvas' o 'FondoNegro' del texto ---
    [Header("Corrección Texto")]
    public Transform textoFlotante; 
    // -----------------------------------------------------------------------

    [Header("Movimiento")]
    public float velocidad = 2.5f;
    public float distanciaParaEmpezar = 3f;
    public float distanciaMinima = 1.5f;
    public float margenHolgura = 0.5f; 

    [Header("Salto")]
    public float fuerzaSalto = 6f; 
    public LayerMask capaSuelo;    
    public Transform detectorPies; 
    public float distanciaDeteccionPared = 0.6f; 

    private bool estaSiguiendo = false;
    private bool mirandoDerecha = true;
    private bool enSuelo = false;
    private bool estaAndando = false; 

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapCircle(detectorPies.position, 0.2f, capaSuelo);

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (!estaSiguiendo && distancia < distanciaParaEmpezar)
        {
            estaSiguiendo = true;
        }

        if (estaSiguiendo)
        {
            bool deboSaltar = enSuelo && HayObstaculoEnfrente() && objetivo.position.y > transform.position.y + 0.5f;

            if (deboSaltar)
            {
                Saltar();
                MoverseHaciaObjetivo();
                estaAndando = true; 
            }
            else
            {
                if (estaAndando)
                {
                    if (distancia <= distanciaMinima)
                    {
                        Parar();
                    }
                    else
                    {
                        MoverseHaciaObjetivo();
                    }
                }
                else
                {
                    if (distancia > distanciaMinima + margenHolgura)
                    {
                        estaAndando = true; 
                    }
                }
            }
        }
    }

    void MoverseHaciaObjetivo()
    {
        float direccionX = (objetivo.position.x > transform.position.x) ? 1 : -1;
        
        rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);

        if (animator != null) animator.SetBool("Caminando", true);
        GestionarGiro(direccionX);
    }

    bool HayObstaculoEnfrente()
    {
        Vector2 direccion = mirandoDerecha ? Vector2.right : Vector2.left;
        Vector2 origenBajo = new Vector2(transform.position.x, transform.position.y - 0.5f); 
        return Physics2D.Raycast(origenBajo, direccion, distanciaDeteccionPared, capaSuelo);
    }

    void Saltar()
    {
        if(Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    void Parar()
    {
        estaAndando = false; 
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (animator != null) animator.SetBool("Caminando", false);
    }

    // --- AQUÍ ESTÁ EL CAMBIO IMPORTANTE ---
    void GestionarGiro(float direccion)
    {
        if (Mathf.Abs(objetivo.position.x - transform.position.x) < 0.2f) return;

        if ((direccion > 0 && !mirandoDerecha) || (direccion < 0 && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            
            // 1. Giramos a Sancho
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;

            // 2. Giramos el texto al revés para que se lea bien
            if (textoFlotante != null)
            {
                Vector3 escalaTexto = textoFlotante.localScale;
                escalaTexto.x *= -1;
                textoFlotante.localScale = escalaTexto;
            }
        }
    }
    // --------------------------------------

    private void OnDrawGizmos()
    {
        if (detectorPies != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(detectorPies.position, 0.2f);
        }
        
        Gizmos.color = Color.red;
        Vector2 direccion = mirandoDerecha ? Vector2.right : Vector2.left;
        Vector2 origenBajo = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Gizmos.DrawRay(origenBajo, direccion * distanciaDeteccionPared);
    }
}