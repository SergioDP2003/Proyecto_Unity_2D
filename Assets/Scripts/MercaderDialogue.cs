using UnityEngine;
using UnityEngine.UI; // Solo si usas Text normal
using TMPro;         // Solo si usas TextMeshPro

public class MercaderDialogue : MonoBehaviour
{
    public GameObject pressEText;      // UI: “Pulsa E para hablar”
    public GameObject dialogueBubble;  // UI: bocadillo “Hola viajero”
    
    private bool cercaDeNPC = false;
    private bool dialogoAbierto = false;

    void Start()
    {
        // Asegurarnos de que empiezan ocultos
        pressEText.SetActive(false);
        dialogueBubble.SetActive(false);
    }

    void Update()
    {
        if (cercaDeNPC)
        {
            // Mostrar texto “Pulsa E para hablar”
            pressEText.SetActive(!dialogoAbierto);

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Alternar diálogo (abrir/cerrar)
                dialogoAbierto = !dialogoAbierto;
                dialogueBubble.SetActive(dialogoAbierto);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mercader"))
        {
            cercaDeNPC = true;
            pressEText.SetActive(true); // Mostrar “Pulsa E”
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mercader"))
        {
            cercaDeNPC = false;
            pressEText.SetActive(false);
            dialogueBubble.SetActive(false);
            dialogoAbierto = false;
        }
    }
}
