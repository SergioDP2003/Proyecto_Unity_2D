using UnityEngine;
// using UnityEngine.SceneManagement; // Descomenta esto cuando uses escenas de verdad

public class TestLauncher : MonoBehaviour
{
    [Header("SECUENCIA DE COMBARES")]
    [Tooltip("Arrastra aquí los 3 enemigos EN ORDEN. Elemento 0 = Combate 1, etc.")]
    public UnitData[] enemySequence; // Lista ordenada

    void Awake()
    {
        // 1. Comprobamos si quedan combates en la lista
        // Miramos si el contador actual es menor que el total de enemigos que has puesto
        if (GameLevel.currentBattleIndex < enemySequence.Length)
        {
            // 2. Elegimos el enemigo que toca según el contador
            GameLevel.enemyToFight = enemySequence[GameLevel.currentBattleIndex];
            
            Debug.Log($"⚔️ INICIANDO COMBATE Nº {GameLevel.currentBattleIndex + 1} contra: {GameLevel.enemyToFight.unitName}");

            // 3. ¡IMPORTANTE! Aumentamos el contador para la próxima vez
            GameLevel.currentBattleIndex++;

            // --- Si estuvieras usando escenas de verdad, aquí harías: ---
            // SceneManager.LoadScene("BattleScene");
            // Como estamos probando en la misma escena, el TurnManager cogerá el dato ahora.
        }
        else
        {
            // Si el contador ya se ha pasado del final de la lista
            Debug.Log("🎉 ¡JUEGO TERMINADO! Has completado los 3 combates.");
            // Aquí podrías cargar una escena de créditos o menú final.
            // Para evitar errores si sigues dando play, limpiamos el enemigo:
            GameLevel.enemyToFight = null;
        }
    }
}