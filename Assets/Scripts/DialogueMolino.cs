using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueMolino : MonoBehaviour
{
    [System.Serializable]
    public class LineaDialogoMolino
    {
        public string nombre;
        public string texto;
        public bool cambiarFondo;
    }

    public LineaDialogoMolino[] dialogo;

    public TMP_Text nombreTexto;
    public TMP_Text dialogoTexto;

    private int indice = 0;

    public UnityEngine.UI.Image fondo;
    public Sprite fondoNormal;
    public Sprite fondoTenebroso;

    public float velocidadTexto = 0.03f;

    private bool escribiendo = false;
    private string textoCompleto = "";


    void Start()
    {
        MostrarLinea();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (escribiendo)
            {
                // Terminar la animación y mostrar frase completa
                StopAllCoroutines();
                dialogoTexto.text = textoCompleto;
                escribiendo = false;
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

            if (dialogo[indice].cambiarFondo)
                CambiarFondo();

            textoCompleto = dialogo[indice].texto;
            StartCoroutine(EscribirTexto());
        }
        else
        {
            Debug.Log("Diálogo terminado");
            SceneManager.LoadScene("Combate_Molino");
        }
    }


    IEnumerator EscribirTexto()
    {
        escribiendo = true;
        dialogoTexto.text = "";

        foreach (char letra in textoCompleto)
        {
            dialogoTexto.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
    }


    void SiguienteLinea()
    {
        indice++;
        MostrarLinea();
    }


    void CambiarFondo()
    {
        if (fondo.sprite == fondoNormal)
            fondo.sprite = fondoTenebroso;
        else
            fondo.sprite = fondoNormal;
    }
}
