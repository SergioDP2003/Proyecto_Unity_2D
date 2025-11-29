using UnityEngine;

public class TextSwitcher : MonoBehaviour
{
    public GameObject[] textBoxes; // Array de cuadros de texto
    private int index = 0;

    void Start()
    {
        // Asegura que solo el primero esté activo
        for (int i = 0; i < textBoxes.Length; i++)
            textBoxes[i].SetActive(i == 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextTextBox();
        }
    }

    void NextTextBox()
    {
        if (index < textBoxes.Length - 1)
        {
            textBoxes[index].SetActive(false);
            index++;
            textBoxes[index].SetActive(true);
        }
    }
}
