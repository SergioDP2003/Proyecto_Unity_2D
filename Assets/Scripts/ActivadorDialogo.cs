using UnityEngine;

public class ActivadorDialogo : MonoBehaviour
{
    [TextArea] // Hace la caja de texto más grande en el inspector
    public string mensaje = "¡Señor, cuidado con los gigantes!";
    
    private bool yaHablado = false; // Para que no lo repita mil veces

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- ZONA DE PRUEBAS (CHIVATO) ---
        // Esto escribirá en la consola (abajo a la izquierda) el nombre de lo que toque la caja.
        Debug.Log("EL TRIGGER HA CHOCADO CON: " + collision.gameObject.name);
        // ---------------------------------

        // Verificamos si es el jugador (Tag: Player) y si no ha hablado ya
        if (collision.CompareTag("Player") && !yaHablado)
        {
            // Buscamos el script de Sancho en la escena
            SanchoHablador sancho = FindFirstObjectByType<SanchoHablador>();
            
            if (sancho != null)
            {
                sancho.DecirFrase(mensaje);
                yaHablado = true; // Marcamos como leído
            }
            else
            {
                Debug.Log("ERROR: El trigger funciona, pero no encuentro a Sancho en la escena.");
            }
        }
    }
}