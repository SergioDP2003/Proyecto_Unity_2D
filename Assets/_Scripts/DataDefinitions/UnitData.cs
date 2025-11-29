using UnityEngine;

[CreateAssetMenu(fileName = "NuevaFicha", menuName = "RPG/Ficha de Personaje", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("Identidad")]
    public string unitName;
    [TextArea] public string description;

    [Header("Diálogos y Narrativa")]
    [TextArea] public string challengeDialogue; // <--- LO QUE DICE AL EMPEZAR ("¡Jamás me vencerás!")
    [TextArea] public string realityDialogue;   // <--- LO QUE DICE AL FINAL ("Ay, que solo era un molino...")

    [Header("Visuales - Transformación")]
    public Sprite unitSprite;       // El Gigante (Combate)
    public Sprite defeatedSprite;   // El Gigante en el suelo (Al ganar)
    public Sprite realitySprite;    // El Molino Roto (La realidad final)
    
    [Header("Escenario")]
    public Sprite backgroundImage;  // El fondo (tu imagen del Molino Malo o el paisaje)

    [Header("Estadísticas")]
    public int maxHP;
    public int damage;
    public int speed;
    
    [Header("Libro de Movimientos")]
    public MoveData[] knownMoves;
}