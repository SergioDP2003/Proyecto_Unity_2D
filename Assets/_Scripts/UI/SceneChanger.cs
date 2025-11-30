using UnityEngine;
using UnityEngine.SceneManagement; // <-- ¡IMPORTANTE! Necesario para cambiar escenas

public class SceneChanger : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribe aquí el nombre EXACTO de la escena de combate a la que quieres ir")]
    public string sceneToLoadName = "Combate_Molino"; // Pon el nombre de TU escena de batalla

    // Esta función es mágica. Unity la llama automáticamente cuando
    // algo entra en el área del "Trigger" de este objeto.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ¿Lo que ha entrado tiene la etiqueta "Player"?
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Jugador detectado! Viajando al combate...");

            // Cargar la nueva escena
            SceneManager.LoadScene(sceneToLoadName);
        }
    }
}