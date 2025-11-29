using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class TurnManager : MonoBehaviour
{
    public BattleState state;

    [Header("ESCENARIO Y UI")]
    public GameObject backgroundObject;        // El fondo normal de batalla
    public GameObject backgroundRealityObject; // <--- NUEVO: El fondo de "Realidad" (Background2)
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [Header("REFERENCIAS JUEGO")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    UnitStats playerUnit;
    UnitStats enemyUnit;
    
    private GameObject enemyGO;
    private SpriteRenderer enemyRenderer;

    private float textDelay = 2.5f;

    void Start()
    {
        // Seguridad extra: Aseguramos que el fondo de realidad empieza apagado
        if (backgroundRealityObject != null)
        {
            backgroundRealityObject.SetActive(false);
        }

        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<UnitStats>();

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<UnitStats>();
        enemyRenderer = enemyGO.GetComponent<SpriteRenderer>();

        if (GameLevel.enemyToFight != null)
        {
            enemyUnit.characterData = GameLevel.enemyToFight;
        }
        enemyUnit.LoadStatsFromData();

        // Configurar Fondo Inicial (El de batalla)
        if (backgroundObject != null && enemyUnit.characterData != null && enemyUnit.characterData.backgroundImage != null)
        {
            AutoScaler scaler = backgroundObject.GetComponent<AutoScaler>();
            if (scaler != null) scaler.SetBackground(enemyUnit.characterData.backgroundImage);
        }

        if (playerHUD != null) playerHUD.SetHUD(playerUnit, this);
        if (enemyHUD != null) enemyHUD.SetHUD(enemyUnit);

        // NARRATIVA INICIAL
        yield return StartCoroutine(ShowDialogueSequence($"¡{playerUnit.unitName} se enfrenta a {enemyUnit.unitName}!"));
        
        if (!string.IsNullOrEmpty(enemyUnit.characterData.challengeDialogue))
        {
             yield return StartCoroutine(ShowDialogueSequence($"{enemyUnit.unitName}: \"{enemyUnit.characterData.challengeDialogue}\""));
        }

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

    IEnumerator PlayerTurnRoutine()
    {
        yield return StartCoroutine(ShowDialogueSequence("Te toca atacar, elige un movimiento..."));
        if (playerHUD != null) playerHUD.SetMenuActive(true);
    }

    public void OnMoveSelected(MoveData moveByIndex)
    {
        if (state != BattleState.PLAYERTURN) return;
        if (playerHUD != null) playerHUD.SetMenuActive(false);
        StartCoroutine(ExecutePlayerMove(moveByIndex));
    }

    IEnumerator ExecutePlayerMove(MoveData move)
    {
        bool hit = CheckAccuracy(move.accuracy);
        string resultMessage = "";

        if (hit)
        {
            bool isDead = enemyUnit.TakeDamage(move.baseDamage);
            if (enemyHUD != null) enemyHUD.SetHP(enemyUnit.currentHP);
            
            resultMessage = $"¡{playerUnit.unitName} usa {move.moveName} y acierta! Causa {move.baseDamage} de daño.";
            yield return StartCoroutine(ShowDialogueSequence(resultMessage));

            if (isDead)
            {
                state = BattleState.WON;
                StartCoroutine(VictorySequence());
                yield break; 
            }
        }
        else
        {
            resultMessage = $"¡{playerUnit.unitName} intenta usar {move.moveName}, pero falla estrepitosamente!";
            yield return StartCoroutine(ShowDialogueSequence(resultMessage));
        }

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurnRoutine());
    }

    IEnumerator EnemyTurnRoutine()
    {
        yield return StartCoroutine(ShowDialogueSequence($"Turno de {enemyUnit.unitName}..."));

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

                if (isDead)
                {
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
            yield return StartCoroutine(ShowDialogueSequence($"{enemyUnit.unitName} no sabe qué hacer y pasa turno."));
        }

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurnRoutine());
    }

    // --- CORRUTINA ACTUALIZADA PARA USAR EL SEGUNDO FONDO ---
    IEnumerator VictorySequence()
    {
        // FASE 1: El gigante cae
        if (enemyUnit.characterData.defeatedSprite != null && enemyRenderer != null)
        {
            enemyRenderer.sprite = enemyUnit.characterData.defeatedSprite;
        }
        yield return StartCoroutine(ShowDialogueSequence($"¡Has derrotado a {enemyUnit.unitName}!"));

        // Pausa dramática
        yield return new WaitForSeconds(1f);

        // FASE 2: LIMPIEZA DE PANTALLA (UI y Enemigo)
        if (enemyGO != null) enemyGO.SetActive(false);
        if (enemyHUD != null) enemyHUD.gameObject.SetActive(false);

        if (playerHUD != null)
        {
            playerHUD.SetMenuActive(false);
            if (playerHUD.hpSlider != null) playerHUD.hpSlider.gameObject.SetActive(false);
            if (playerHUD.nameText != null) playerHUD.nameText.gameObject.SetActive(false);
        }

        // --- FASE 3: LA REVELACIÓN (Activamos el segundo fondo) ---
        
        // CAMBIO AQUÍ: Simplemente encendemos el objeto que ya tiene la imagen correcta
        if (backgroundRealityObject != null)
        {
            backgroundRealityObject.SetActive(true);
        }
        // (Como está en una capa superior, tapará al anterior automáticamente)

        // Pequeña pausa para asimilar el nuevo fondo antes del texto
        yield return new WaitForSeconds(0.5f);

        // Diálogo final de realidad
        string finalReflexion = !string.IsNullOrEmpty(enemyUnit.characterData.realityDialogue) 
            ? enemyUnit.characterData.realityDialogue 
            : "No eran gigantes... solo era mi imaginación.";
            
        yield return StartCoroutine(ShowDialogueSequence(finalReflexion));

        // FASE 4: Fin real
        Debug.Log("COMBATE TERMINADO - Cargando mapa...");
        // SceneManager.LoadScene("TuEscenaDeMapa"); 
    }

    IEnumerator ShowDialogueSequence(string message)
    {
        if (playerHUD != null)
        {
            playerHUD.ShowDialogue(message);
            yield return new WaitForSeconds(textDelay);
            playerHUD.SetDialogueActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    bool CheckAccuracy(int accuracy) { return Random.Range(1, 101) <= accuracy; }

    void EndBattle()
    {
        if (state == BattleState.LOST)
        {
            Debug.Log("DERROTA...");
            StartCoroutine(ShowDialogueSequence("Don Quijote ha caído vencido..."));
            // SceneManager.LoadScene("MenuPrincipal");
        }
    }
}