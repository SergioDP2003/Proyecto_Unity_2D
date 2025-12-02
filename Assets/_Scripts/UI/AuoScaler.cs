using UnityEngine;

[ExecuteAlways] // ¡Truco! Esto hace que funcione incluso sin darle al Play
[RequireComponent(typeof(SpriteRenderer))]
public class AutoScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float cameraHeight;
    private float cameraWidth;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FitToScreen();
    }

    // Esta función se ejecuta en cada fotograma del juego
    void Update()
    {
        FitToScreen();
    }

    public void SetBackground(Sprite newSprite)
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
            FitToScreen();
        }
    }

    // La matemáticas para estirar la imagen
    private void FitToScreen()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;

        // 1. Calculamos el tamaño que ve la cámara
        cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        
        // 2. Cogemos el tamaño de la imagen
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // 3. Calculamos la escala necesaria para cada eje
        Vector2 scale = transform.localScale;
        
        // OPCIÓN: Rellenar pantalla (Zoom para que no queden huecos)
        // Usamos el factor mayor para asegurarnos que cubrimos todo
        float scaleFactorX = cameraSize.x / spriteSize.x;
        float scaleFactorY = cameraSize.y / spriteSize.y;
        
        float finalScale = Mathf.Max(scaleFactorX, scaleFactorY);

        transform.localScale = new Vector3(finalScale, finalScale, 1);
    }
}