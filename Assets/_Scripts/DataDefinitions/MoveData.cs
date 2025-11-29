using UnityEngine;

// Esto nos permite crear "Movimientos" con clic derecho en Unity
[CreateAssetMenu(fileName = "NuevoMovimiento", menuName = "RPG/Movimiento de Combate")]
public class MoveData : ScriptableObject
{
    [Header("Info del Movimiento")]
    public string moveName;      // Ej: "Lanza en Ristre"
    [TextArea]
    public string description;   // Ej: "Un ataque directo. Puede fallar si estás mareado."

    [Header("Estadísticas")]
    public int baseDamage;       // Daño base del movimiento (Ej: 50)
    [Range(0, 100)]
    public int accuracy;         // Precisión de 0 a 100% (Ej: 90)

    // Más adelante aquí podrías añadir:
    // - Coste de Maná/Locura
    // - Tipo (Físico, Mental...)
    // - Efectos secundarios (Aturdir, sangrar...)
    // - Animación asociada
}