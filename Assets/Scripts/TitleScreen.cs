using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Botón Play (por ahora no hace nada)
    public void PlayGame()
    {
        Debug.Log("Play pressed — todavía no hay escena que cargar.");
        // Aquí más adelante pondrás SceneManager.LoadScene("TuEscena");
    }

    // Botón Exit
    public void QuitGame()
    {
        Debug.Log("Quit pressed — cerrando el juego.");
        Application.Quit();
    }
}
