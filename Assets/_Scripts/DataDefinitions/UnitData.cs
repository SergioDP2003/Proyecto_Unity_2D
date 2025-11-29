using UnityEngine;

// Esta etiqueta [CreateAssetMenu] es la que hace la magia.
// Permite que hagas Clic Derecho en Unity -> Create -> RPG -> Ficha de Personaje
[CreateAssetMenu(fileName = "NuevaFicha", menuName = "RPG/Ficha de Personaje", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("Identidad")]
    public string unitName;        // Ej: "Don Quijote" o "Molino Malvado"
    [TextArea]
    public string description;     // Ej: "Cree que es un gigante, pero tiene aspas."

    [Header("Aspecto Visual")]
    public Sprite unitSprite;      // Arrastra aquí la imagen (PNG) del personaje
    public Sprite backgroundImage; // Arrastra aquí la imagen (PNG) del fondo
    public Sprite enemySprite;  // Arrastra aquí la imagen (PNG) del enemigo
    public Sprite interfaz;     // Arrastra aquí la imagen (PNG) de la interfaz

    [Header("Estadísticas de Combate")]
    public int maxHP;              // Vida máxima (Ej: 500 para Quijote, 100 para enemigos)
    public int damage;             // Daño base por golpe
    public int speed;              // Velocidad: Quien tenga más, ataca primero en el turno

    [Header("Libro de Movimientos")]
    public MoveData[] knownMoves;
}