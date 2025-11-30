using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("CONFIGURACIÓN")]
    [Tooltip("Arrastra aquí la Ficha (ScriptableObject) del personaje")]
    public UnitData characterData; 

    [Header("DEBUG (Opcional)")]
    public bool isInvincible = false; // Marca esto si quieres que el Quijote no muera nunca en pruebas

    // --- Variables de Estado ---
    [HideInInspector] public string unitName;
    [HideInInspector] public int damage;
    [HideInInspector] public int maxHP;
    public int currentHP; // Lo dejo visible para que lo veas en el Inspector
    [HideInInspector] public int speed;

    // Referencias internas
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Buscamos el componente que dibuja al personaje
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Intentamos cargar datos al nacer. 
        // Si no hay ficha, no pasará nada gracias al cambio de abajo.
        LoadStatsFromData();
    }

    /// <summary>
    /// Vuelca toda la información de la 'Ficha' (UnitData) a este objeto vivo.
    /// </summary>
    public void LoadStatsFromData()
    {
        // --- CAMBIO CLAVE AQUÍ ---
        // Si characterData es null (vacío), simplemente salimos de la función sin dar error.
        // Esto permite que el TurnManager cree el muñeco vacío primero y le asigne los datos después.
        if (characterData == null)
        {
            return; 
        }

        // 1. Copiar estadísticas numéricas y textos
        unitName = characterData.unitName;
        damage = characterData.damage;
        maxHP = characterData.maxHP;
        speed = characterData.speed; 

        // 2. Inicializar la vida al máximo
        currentHP = maxHP;

        // 3. Cambiar el aspecto visual automáticamente
        if (characterData.unitSprite != null)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = characterData.unitSprite;
            }
            else
            {
                // Solo avisamos si tienes datos de imagen pero te falta el componente SpriteRenderer
                Debug.LogWarning($"[UnitStats] '{gameObject.name}' tiene sprite en la ficha, pero no tiene SpriteRenderer.");
            }
        }
    }

    /// <summary>
    /// Aplica daño al personaje.
    /// </summary>
    public bool TakeDamage(int dmg)
    {
        if (isInvincible) return false; // Modo Dios

        currentHP -= dmg;
        
        // Evitamos números negativos en la vida
        if (currentHP < 0) currentHP = 0;

        Debug.Log($"{unitName} recibió {dmg} de daño. Vida restante: {currentHP}/{maxHP}");

        if (currentHP == 0)
        {
            return true; // Murió
        }
        return false; // Sigue vivo
    }

    /// <summary>
    /// Cura al personaje sin sobrepasar su vida máxima.
    /// </summary>
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        Debug.Log($"{unitName} se curó {amount} puntos. Vida: {currentHP}/{maxHP}");
    }
}