using UnityEngine;
using UnityEngine.SceneManagement; // 1. IMPORTANTE: Necesario para cambiar de escena

public class NPCInteractuable : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject avisoBoton; // El texto de "Pulsa E"

    [Header("Configuración")]
    [TextArea] public string mensajeDelPastor = "¡COMO TE ATREVES A MATAR MIS OVEJAS, LUCHA CONMIGO!";
    
    // 2. Aquí escribes el nombre de la escena de pelea en el Inspector
    public string nombreEscenaBatalla = "DialogueCampfire"; 
    
    private bool jugadorEnRango = false;
    private BocadilloSimple miBocadillo; 
    private bool yaInteractuado = false; // Para que no pulse E veinte veces seguidas

    void Start()
    {
        miBocadillo = GetComponent<BocadilloSimple>();
    }

    void Update()
    {
        // Añadimos !yaInteractuado para que solo funcione una vez
        if (jugadorEnRango && Input.GetKeyDown(KeyCode.E) && !yaInteractuado)
        {
            StartCoroutine(SecuenciaBatalla());
        }
    }

    // 3. La Corrutina que gestiona la espera y el cambio
    System.Collections.IEnumerator SecuenciaBatalla()
    {
        yaInteractuado = true; // Bloqueamos para no repetir
        
        if (miBocadillo != null)
        {
            miBocadillo.Hablar(mensajeDelPastor);
            avisoBoton.SetActive(false); // Ocultamos el "Pulsa E"
        }

        // 4. Esperamos 3 segundos (o lo que quieras) para leer el grito
        yield return new WaitForSeconds(3f);

        // 5. Cambiamos de escena si has puesto un nombre
        if (nombreEscenaBatalla != "")
        {
            Debug.Log("Cargando batalla...");
            SceneManager.LoadScene(nombreEscenaBatalla);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // OJO AQUÍ: Asegúrate de que tu Quijote tiene el Tag "Player" o "Player1"
        // Si antes usaste "Player1", cámbialo aquí también.
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = true;
            if (!yaInteractuado) avisoBoton.SetActive(true);
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