using UnityEngine;
using UnityEngine.SceneManagement; 

public class Fin : MonoBehaviour
{

    // Botón Exit
    public void QuitGame()
    {
        Debug.Log("Quit pressed — cerrando el juego.");
        SceneManager.LoadScene("MainMenu"); 
    }
}
