using System.Collections;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class TurnManager : MonoBehaviour
{
    public BattleState state;

    [Header("ESCENARIO Y UI")]
    public GameObject backgroundObject;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("REFERENCIAS JUEGO")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    UnitStats playerUnit;
    UnitStats enemyUnit;

    // Variable para controlar el tiempo de lectura de los mensajes
    private float textDelay = 2.5f; 

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

   IEnumerator SetupBattle()
    {
        // (Esta parte inicial de crear personajes es IGUAL que antes)
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<UnitStats>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<UnitStats>();

        if (GameLevel.enemyToFight != null) enemyUnit.characterData = GameLevel.enemyToFight;
        enemyUnit.LoadStatsFromData();

        // --- AQUÍ ESTÁ EL CAMBIO DEL FONDO ---
        // Verificamos que tenemos el objeto fondo y que el enemigo tiene imagen de fondo
        if (backgroundObject != null && enemyUnit.characterData.backgroundImage != null)
        {
            // 1. Buscamos el script mágico 'AutoScaler' en el objeto del fondo
            AutoScaler scaler = backgroundObject.GetComponent<AutoScaler>();

            if (scaler != null)
            {
                // 2. Si existe, le pasamos la imagen y dejamos que él calcule el tamaño
                scaler.SetBackground(enemyUnit.characterData.backgroundImage);
            }
            else
            {
                // Aviso de seguridad por si se te olvidó poner el script en Unity
                Debug.LogWarning("¡Cuidado! El objeto 'Background' no tiene el componente 'AutoScaler' asignado.");
            }
        }
        // ------------------------------------

        if (playerHUD != null) playerHUD.SetHUD(playerUnit, this);
        if (enemyHUD != null) enemyHUD.SetHUD(enemyUnit);

        // (El resto sigue IGUAL que antes)
        // Usamos el panel de diálogo para anunciar el inicio
        yield return StartCoroutine(ShowDialogueSequence($"¡{playerUnit.unitName} se enfrenta a {enemyUnit.unitName}!"));

        // Decidir turnos y empezar la secuencia correspondiente
        if (playerUnit.speed >= enemyUnit.speed)
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurnRoutine());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurnRoutine());
        }
    }
    // --- NUEVA CORRUTINA: SECUENCIA DEL TURNO DEL JUGADOR ---
    IEnumerator PlayerTurnRoutine()
    {
        // 1. Mostrar mensaje inicial
        yield return StartCoroutine(ShowDialogueSequence("Te toca atacar, elige un movimiento..."));

        // 2. Mostrar botones
        if (playerHUD != null) playerHUD.SetMenuActive(true);
        
        // El juego se queda aquí esperando a que pulses un botón.
        // Cuando pulsas, el botón llama a 'OnMoveSelected'
    }

    // Esta función la llaman los botones al hacer clic
    public void OnMoveSelected(MoveData moveByIndex)
    {
        if (state != BattleState.PLAYERTURN) return;
        
        // 3. Ocultar botones inmediatamente
        if (playerHUD != null) playerHUD.SetMenuActive(false);

        // 4. Iniciar la secuencia de ataque
        StartCoroutine(ExecutePlayerMove(moveByIndex));
    }

    // --- SECUENCIA DE ATAQUE DEL JUGADOR (Modificada con diálogos) ---
    IEnumerator ExecutePlayerMove(MoveData move)
    {
        bool hit = CheckAccuracy(move.accuracy);
        string resultMessage = "";

        if (hit)
        {
            bool isDead = enemyUnit.TakeDamage(move.baseDamage);
            if (enemyHUD != null) enemyHUD.SetHP(enemyUnit.currentHP);
            // Construimos el mensaje de éxito
            resultMessage = $"¡{playerUnit.unitName} usa {move.moveName} y acierta! Causa {move.baseDamage} de daño.";
            
            // Mostramos el resultado y esperamos a que se lea
            yield return StartCoroutine(ShowDialogueSequence(resultMessage));

            if (isDead) {
                state = BattleState.WON;
                EndBattle();
                yield break;
            }
        }
        else
        {
            // Construimos el mensaje de fallo
            resultMessage = $"¡{playerUnit.unitName} intenta usar {move.moveName}, pero falla estrepitosamente!";
            // Mostramos el resultado y esperamos
            yield return StartCoroutine(ShowDialogueSequence(resultMessage));
        }

        // Cambio de turno
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurnRoutine());
    }


    // --- SECUENCIA DEL TURNO DEL ENEMIGO (Modificada con diálogos) ---
    IEnumerator EnemyTurnRoutine()
    {
        // 1. Anunciar turno enemigo
        yield return StartCoroutine(ShowDialogueSequence($"Turno de {enemyUnit.unitName}..."));

        // IA: Elegir movimiento
        MoveData[] enemyMoves = enemyUnit.characterData.knownMoves;
        if (enemyMoves != null && enemyMoves.Length > 0)
        {
            MoveData selectedMove = enemyMoves[Random.Range(0, enemyMoves.Length)];
            bool hit = CheckAccuracy(selectedMove.accuracy);
            string resultMessage = "";

            if (hit)
            {
                bool isDead = playerUnit.TakeDamage(selectedMove.baseDamage);
                if (playerHUD != null) playerHUD.SetHP(playerUnit.currentHP);
                resultMessage = $"¡{enemyUnit.unitName} usa {selectedMove.moveName}! Te hace {selectedMove.baseDamage} de daño.";
                
                yield return StartCoroutine(ShowDialogueSequence(resultMessage));

                if (isDead) {
                    state = BattleState.LOST;
                    EndBattle();
                    yield break;
                }
            }
            else
            {
                 resultMessage = $"¡{enemyUnit.unitName} usa {selectedMove.moveName}, pero falla!";
                 yield return StartCoroutine(ShowDialogueSequence(resultMessage));
            }
        }
        else
        {
             // Si el enemigo no tiene ataques configurados
             yield return StartCoroutine(ShowDialogueSequence($"{enemyUnit.unitName} no sabe qué hacer y pasa turno."));
        }

        // Volvemos al turno del jugador
        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurnRoutine());
    }

    // --- HELPER: Corrutina maestra para mostrar diálogos y esperar ---
    // Esta función se encarga de abrir el panel, poner el texto, esperar y cerrarlo.
    IEnumerator ShowDialogueSequence(string message)
    {
        // Usamos el HUD del jugador para mostrar los mensajes globales
        if (playerHUD != null)
        {
            playerHUD.ShowDialogue(message);
            // Esperamos X segundos para que el jugador lea
            yield return new WaitForSeconds(textDelay);
            // Ocultamos el panel
            playerHUD.SetDialogueActive(false);
            // Pequeña pausa extra después de cerrar el panel para que no sea brusco
            yield return new WaitForSeconds(0.5f);
        }
    }

    // (CheckAccuracy y EndBattle siguen IGUAL que antes, no hace falta cambiarlos)
    bool CheckAccuracy(int accuracy) { /* ... */ return Random.Range(1, 101) <= accuracy; }
    void EndBattle() { /* ... */ }
}