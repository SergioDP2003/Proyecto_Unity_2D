using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueCampfire : MonoBehaviour
{
    [System.Serializable]
    public class LineaDialogo
    {
        public string nombre;
        public string texto;
    }

    public LineaDialogo[] dialogo;

    public TMP_Text nombreTexto;
    public TMP_Text dialogoTexto;

    private int indice = 0;

    void Start()
    {
        MostrarLinea();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SiguienteLinea();
        }
    }

    void MostrarLinea()
    {
        if (indice < dialogo.Length)
        {
            nombreTexto.text = dialogo[indice].nombre;
            dialogoTexto.text = dialogo[indice].texto;
        }
        else
        {
            // Aquí puedes cambiar de escena o cerrar diálogo
            Debug.Log("Diálogo terminado");
            SceneManager.LoadScene("Cap2Background");
        }
    }

    void SiguienteLinea()
    {
        indice++;
        MostrarLinea();
    }
}
