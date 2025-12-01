using UnityEngine;

public class NPCInteractuable : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject avisoBoton; // El texto de "Pulsa E"

    [Header("Configuración")]
    [TextArea] public string mensajeDelPastor = "¡COMO TE ATREVES A MATAR MIS OVEJAS, LUCHA CONMIGO!";
    
    private bool jugadorEnRango = false;
    private BocadilloSimple miBocadillo; // Referencia a su propia voz

    void Start()
    {
        // Buscamos el script de hablar en este mismo objeto (El Pastor)
        miBocadillo = GetComponent<BocadilloSimple>();
    }

    void Update()
    {
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E))
        {
            if (miBocadillo != null)
            {
                miBocadillo.Hablar(mensajeDelPastor);
                
                // Opcional: Ocultar el "Pulsa E" mientras habla para que no moleste
                avisoBoton.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = true;
            avisoBoton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = false;
            avisoBoton.SetActive(false);
        }
    }
}