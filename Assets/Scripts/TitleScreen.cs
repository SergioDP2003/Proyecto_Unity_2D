using UnityEngine;
using UnityEngine.SceneManagement; // <-- IMPORTANTE para cargar escenas

public class TitleScreen : MonoBehaviour
{
    // Botón Play
    public void PlayGame()
    {
        Debug.Log("Play pressed — cargando nueva escena...");
        SceneManager.LoadScene("DialogueNarrator");  
    }

    // Botón Exit
    public void QuitGame()
    {
        Debug.Log("Quit pressed — cerrando el juego.");
        Application.Quit();
    }
}
