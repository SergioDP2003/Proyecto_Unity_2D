using UnityEngine;
using UnityEngine.UI; // Necesario para los Sliders y Textos viejos
using System.Collections.Generic;
using TMPro; // Necesario para TextMeshPro

public class BattleHUD : MonoBehaviour
{
    [Header("Elementos Básicos")]
    // Si usas texto normal para el nombre, usa 'Text'. Si usas TMP, pon 'TextMeshProUGUI'.
    public Text nameText; 
    public Slider hpSlider;

    [Header("MENÚ DE ACCIONES (Solo Jugador)")]
    public GameObject actionMenuPanel;
    // Lista de los botones físicos
    public Button[] moveButtons; 
    // Lista de los textos (TMP) dentro de esos botones
    public TextMeshProUGUI[] moveTexts; 

    [Header("PANEL DE DIÁLOGO (Mensajes)")] // <--- NUEVA SECCIÓN
    public GameObject dialoguePanel;        // <--- Referencia al panel de arriba
    public TextMeshProUGUI dialogueText;    // <--- Referencia al texto dentro del panel

    // Referencia al jefe para avisarle cuando pulsamos botones
    private TurnManager turnManager;

    // Configuración inicial al arrancar el combate
    public void SetHUD(UnitStats unit, TurnManager managerReference = null)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        // Si nos pasan el manager (significa que es el HUD del jugador)
        if (managerReference != null)
        {
            turnManager = managerReference;
            // Configuramos los nombres de los botones de ataque
            SetupMoveButtons(unit.characterData.knownMoves);
        }
        
        // Aseguramos que al empezar, los menús y textos están ocultos
        SetMenuActive(false);
        SetDialogueActive(false); // <--- Nuevo: Ocultar diálogo al inicio
    }

    // Actualizar la barra de vida
    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    // Mostrar/Ocultar los botones de ataque
    public void SetMenuActive(bool isActive)
    {
        if (actionMenuPanel != null) actionMenuPanel.SetActive(isActive);
    }

    // --- NUEVAS FUNCIONES PARA EL DIÁLOGO ---

    // Función para mostrar un mensaje concreto en el panel superior
    public void ShowDialogue(string message)
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialogueText.text = message;
            SetDialogueActive(true); // Mostramos el panel
        }
    }

    // Función para activar/desactivar el panel de diálogo
    public void SetDialogueActive(bool isActive)
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(isActive);
    }

    // -----------------------------------------

    // Configura los botones de ataque dinámicamente
    void SetupMoveButtons(MoveData[] moves)
    {
        for (int i = 0; i < moveButtons.Length; i++)
        {
            // Si hay un movimiento válido para este hueco...
            if (i < moves.Length && moves[i] != null)
            {
                moveButtons[i].gameObject.SetActive(true);
                
                // Ponemos el nombre del ataque en el botón
                moveTexts[i].text = moves[i].moveName;

                // Guardamos el movimiento 'm' para la función lambda
                MoveData m = moves[i]; 
                
                // Limpiamos funciones viejas y añadimos la nueva al hacer clic
                moveButtons[i].onClick.RemoveAllListeners();
                moveButtons[i].onClick.AddListener(() => turnManager.OnMoveSelected(m));
            }
            else
            {
                // Si no hay movimiento, ocultamos el botón sobrante
                moveButtons[i].gameObject.SetActive(false);
            }
        }
    }
}