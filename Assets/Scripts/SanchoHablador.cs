using UnityEngine;
using TMPro; // Necesario para el texto
using System.Collections;

public class SanchoHablador : MonoBehaviour
{
    [Header("Arrastra aquí el Canvas que creaste dentro de Sancho")]
    public GameObject bocadilloCanvas; 
    public TextMeshProUGUI textoComponente; // Si usas texto normal, pon 'Text' en vez de 'TextMeshProUGUI'

    // Esta función la llamará el Trigger invisible
    public void DecirFrase(string frase)
    {
        // 1. Ponemos el texto
        textoComponente.text = frase;
        
        // 2. Activamos el bocadillo visualmente
        bocadilloCanvas.SetActive(true);

        // 3. Iniciamos la cuenta atrás para borrarlo
        StopAllCoroutines(); // Por si ya estaba hablando, reseteamos el tiempo
        StartCoroutine(EsconderFraseDespuesDeTiempo(4f)); // 4 segundos
    }

    IEnumerator EsconderFraseDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        bocadilloCanvas.SetActive(false); // Apagamos el bocadillo
    }
}