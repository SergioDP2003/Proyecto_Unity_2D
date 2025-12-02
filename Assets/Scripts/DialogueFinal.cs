using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueFinal : MonoBehaviour
{
    [System.Serializable]
    public class LineaDialogoFinal
    {
        public string nombre;
        public string texto;
    }

    public LineaDialogoFinal[] dialogo;

    public TMP_Text nombreTexto;
    public TMP_Text dialogoTexto;

    public float velocidadEscritura = 0.03f; // tiempo entre letras

    private int indice = 0;
    private Coroutine escribiendo = null;    // Referencia a la corrutina activa

    void Start()
    {
        MostrarLinea();
    }

    void Update()
    {
        // Si pulsas espacio avanza al siguiente diálogo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Si se está escribiendo, mostrar la línea completa instantáneamente
            if (escribiendo != null)
            {
                StopCoroutine(escribiendo);
                dialogoTexto.text = dialogo[indice].texto;
                escribiendo = null;
            }
            else
            {
                SiguienteLinea();
            }
        }
    }

    void MostrarLinea()
    {
        if (indice < dialogo.Length)
        {
            nombreTexto.text = dialogo[indice].nombre;

            // Si hay una corrutina escribiendo, la detenemos
            if (escribiendo != null)
            {
                StopCoroutine(escribiendo);
            }

            // Arranca escritura letra por letra
            escribiendo = StartCoroutine(EscribirTexto(dialogo[indice].texto));
        }
        else
        {
            Debug.Log("Diálogo terminado");
            SceneManager.LoadScene("Final");
        }
    }

    void SiguienteLinea()
    {
        indice++;
        MostrarLinea();
    }

    IEnumerator EscribirTexto(string frase)
    {
        dialogoTexto.text = "";

        foreach (char letra in frase)
        {
            dialogoTexto.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        // Escritura terminada → permitir avanzar
        escribiendo = null;
    }
}
