using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoScaler : MonoBehaviour
{
    [Header("Configuración de Escalado")]
    [Tooltip("Si está activado, la imagen se verá entera sin hacer zoom, pero pueden quedar bordes vacíos. Si está desactivado, hará zoom para cubrir toda la pantalla.")]
    public bool fitToScreen = true; // <--- ¡LA NUEVA OPCIÓN! Por defecto la ponemos a True para evitar el zoom

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBackground(Sprite newSprite)
    {
        if (newSprite == null) return;

        // 1. Asignamos la nueva imagen
        spriteRenderer.sprite = newSprite;

        // 2. Reseteamos la escala
        transform.localScale = Vector3.one;

        // 3. Calculamos tamaño de la imagen
        float width = spriteRenderer.bounds.size.x;
        float height = spriteRenderer.bounds.size.y;

        // 4. Calculamos tamaño de la pantalla
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // 5. Calculamos factores de estiramiento
        float scaleX = worldScreenWidth / width;
        float scaleY = worldScreenHeight / height;

        // --- 6. ELEGIMOS EL MODO DE ESCALADO (AQUÍ ESTÁ LA MAGIA) ---
        float finalScale;
        if (fitToScreen)
        {
            // Modo "Ajustar" (Sin zoom exagerado): Usamos el factor más PEQUEÑO.
            // Esto asegura que toda la imagen cabe en la pantalla.
            finalScale = Mathf.Min(scaleX, scaleY);
        }
        else
        {
            // Modo "Rellenar" (El de antes): Usamos el factor más GRANDE.
            // Esto asegura que cubrimos toda la pantalla, aunque hagamos zoom.
            finalScale = Mathf.Max(scaleX, scaleY);
        }
        // -----------------------------------------------------------

        // 7. Aplicamos la escala
        transform.localScale = new Vector3(finalScale, finalScale, 1f);

        // Centramos el fondo
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 10f);
    }
}