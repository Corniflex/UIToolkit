using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private UIDocument dialogueUIDocument;
    [SerializeField] private GameObject dialogueMenu;
    [SerializeField] private Sprite portraitToUse;
    private VisualElement dialogueRootElement;
    void Start()
    {
        OpenDialogueUI();
    }
    
    private void OpenDialogueUI()
    {
        dialogueMenu.SetActive(true);
        dialogueRootElement = dialogueUIDocument.rootVisualElement;
        
        var labelToFill = dialogueRootElement.Q<Label>("CharacterName");
        labelToFill.text = "Char Name";

        labelToFill = dialogueRootElement.Q<Label>("DialogueText");
        labelToFill.text = "Dialogue line displayed :)";

        var portrait = dialogueRootElement.Q<VisualElement>("CharacterPortrait");
        portrait.style.backgroundImage = new StyleBackground(portraitToUse);
    }

    private void CloseDialogueUI()
    {
        dialogueMenu.SetActive(false);
    }
}
