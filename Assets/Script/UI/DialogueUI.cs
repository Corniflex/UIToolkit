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

    public DialogueSystem.Dialogue currentDialogue;
    public bool isOpen;
    
    public void OpenDialogueUI(DialogueSO dialogueSo)
    {
        dialogueMenu.SetActive(true);
        currentDialogue = dialogueSo.dialogue;
        dialogueRootElement = dialogueUIDocument.rootVisualElement;
        
        var labelToFill = dialogueRootElement.Q<Label>("CharacterName");
        labelToFill.text = dialogueSo.characterName;

        labelToFill = dialogueRootElement.Q<Label>("DialogueText");
        labelToFill.text = currentDialogue.text;

        var portrait = dialogueRootElement.Q<VisualElement>("CharacterPortrait");
        portrait.style.backgroundImage = new StyleBackground(dialogueSo.characterSprite);

        if (!currentDialogue.hasChoice)
            DisableChoiceBox();
        else
        {
            EnableChoiceBox();
        }
        
        isOpen = true;
    }

    public void UpdateToNext()
    {
        if (currentDialogue.isLastDialogue)
        {
            CloseDialogueUI();
            return;
        }

        if (currentDialogue.hasChoice)
            return;
        
        currentDialogue = currentDialogue.choice.choiceFollow[0];
        var labelToFill = dialogueRootElement.Q<Label>("DialogueText");
        labelToFill.text = currentDialogue.text;
        if (currentDialogue.hasChoice)
            EnableChoiceBox();
        else
            DisableChoiceBox();
    }

    public void UpdateToNext(ClickEvent e, int choice)
    {
        currentDialogue = currentDialogue.choice.choiceFollow[choice];
        var labelToFill = dialogueRootElement.Q<Label>("DialogueText");
        labelToFill.text = currentDialogue.text;
        if(currentDialogue.hasChoice)
            EnableChoiceBox();
        else
            DisableChoiceBox();
    }

    private void EnableChoiceBox()
    {
        //dialogueRootElement.Q<GroupBox>("ChoiceGroup").style.display = DisplayStyle.Flex;
        for (int i = 0; i < currentDialogue.choice.choiceFollow.Length; i++)
        {
            var buttonToSet = dialogueRootElement.Q<Button>($"Choice{i + 1}");
            buttonToSet.style.display = DisplayStyle.Flex;
            buttonToSet.text = currentDialogue.choice.options[i];
            buttonToSet.RegisterCallback<ClickEvent, int>(UpdateToNext, i);
        }

        for (int i = currentDialogue.choice.choiceFollow.Length; i < 3; i++)
        {
            dialogueRootElement.Q<Button>($"Choice{i + 1}").style.display = DisplayStyle.None;
        }
    }

    private void DisableChoiceBox()
    {
        //dialogueRootElement.Q<GroupBox>("ChoiceGroup").style.display = DisplayStyle.None;
        for(int i = 0; i < 3; i++)
        {
            var buttonToSet = dialogueRootElement.Q<Button>($"Choice{i + 1}");
            buttonToSet.style.display = DisplayStyle.None;
        }
    }
    
    public void CloseDialogueUI()
    {
        isOpen = false;
        dialogueMenu.SetActive(false);
    }
}
