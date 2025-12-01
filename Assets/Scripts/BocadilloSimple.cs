using UnityEngine;
using TMPro;
using System.Collections;

public class BocadilloSimple : MonoBehaviour
{
    [Header("Arrastra aquí el Canvas del DIÁLOGO (No el de la E)")]
    public GameObject canvasBocadillo; 
    public TextMeshProUGUI textoComponente;

    public void Hablar(string frase)
    {
        // 1. Ponemos el texto
        textoComponente.text = frase;
        
        // 2. Encendemos el bocadillo
        canvasBocadillo.SetActive(true);

        // 3. Reiniciamos el temporizador para que se quite solo
        StopAllCoroutines();
        StartCoroutine(EsconderBocadillo(4f)); // Dura 4 segundos
    }

    IEnumerator EsconderBocadillo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        canvasBocadillo.SetActive(false);
    }
}