using UnityEngine;
using UnityEngine.UI; // Necesario para controlar la UI
using System.Collections;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    public GameObject[] corazones; // Arrastra aquí las 3 imágenes de la UI
    public int vidas = 3;

    [Header("Componentes")]
    private SpriteRenderer spriteRenderer;
    private bool esInvencible = false; // Para no perder 3 vidas de golpe

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RecibirDano()
    {
        // Si ya está herido o muerto, no hacemos nada
        if (esInvencible || vidas <= 0) return;

        vidas--; // Restamos 1 vida

        // Apagamos el corazón correspondiente
        // Si teníamos 3 vidas y bajamos a 2, apagamos el índice 2 (que es el tercer corazón)
        if (vidas >= 0 && vidas < corazones.Length)
        {
            corazones[vidas].SetActive(false);
        }

        if (vidas <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(EfectoHerido());
        }
    }

    IEnumerator EfectoHerido()
    {
        esInvencible = true; // Se vuelve inmortal temporalmente
        spriteRenderer.color = Color.red; // Se pone rojo

        yield return new WaitForSeconds(1f); // Espera medio segundo

        spriteRenderer.color = Color.white; // Vuelve a la normalidad
        esInvencible = false; // Ya le pueden pegar otra vez
    }

    void Morir()
    {
        spriteRenderer.color = Color.gray; // Se pone gris (o animación de muerte)
        Debug.Log("¡GAME OVER! Don Quijote ha caído.");
        // Aquí podrías recargar la escena:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}