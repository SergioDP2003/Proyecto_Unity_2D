using UnityEngine;
using System.Collections;

public class FuegoDagnino : MonoBehaviour
{
    // Usamos OnTriggerStay2D. Esto se ejecuta en cada frame mientras
    // otro collider (el del jugador) esté "dentro" del trigger del fuego.
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 1. Comprobamos si lo que está dentro del fuego es el jugador.
        // Asegúrate de que tu Don Quijote tenga el Tag "Player1"
        if (collision.CompareTag("Player"))
        {
            // 2. Buscamos el script que controla la vida del jugador.
            VidaJugador vidaScript = collision.GetComponent<VidaJugador>();

            // 3. Si encontramos el script, le decimos que reciba daño.
            if (vidaScript != null)
            {
                // NOTA: No hace falta un temporizador aquí.
                // Como tu script 'VidaJugador' ya vuelve al jugador invencible
                // durante 0.5 segundos cuando recibe un golpe, el fuego intentará
                // hacer daño en cada frame, pero el jugador solo lo aceptará
                // una vez cada medio segundo. ¡Funciona automático!
                vidaScript.RecibirDano();
            }
        }
    }
}