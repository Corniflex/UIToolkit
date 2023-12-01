using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private DialogueUI dialogueUI;

    private Rigidbody rb;
    private DialogueSO characterDialogueSO;
    private PlayerInput playerInput;
    
    private void Awake()
    {
        playerInput = new();
        playerInput.Controls.Enable();
        rb = GetComponent <Rigidbody>();
    }

    private void Update()
    {
        InputControls();
        Pause();
        InitiateDialogue();
    }

    private void InputControls()
    {
        if (playerInput.Controls.Up.IsPressed())
            rb.AddForce(Vector3.forward * .3f, ForceMode.Acceleration);
        if (playerInput.Controls.Down.IsPressed())
            rb.AddForce(Vector3.back * .3f, ForceMode.Acceleration);
        if (playerInput.Controls.Left.IsPressed())
            rb.AddForce(Vector3.left * .3f, ForceMode.Acceleration);
        if (playerInput.Controls.Right.IsPressed())
            rb.AddForce(Vector3.right * .3f, ForceMode.Acceleration);

        if (rb.velocity.magnitude > 2.5f) rb.velocity = rb.velocity.normalized * 2.5f;
    }

    private void InitiateDialogue()
    {
        if (playerInput.Controls.DebugKey.triggered)
        {
            if (dialogueUI.isOpen)
            {
                dialogueUI.UpdateToNext();
            }
            else
                dialogueUI.OpenDialogueUI(characterDialogueSO);
        }
    }

    private void Pause()
    {
        if (playerInput.Controls.Pause.triggered)
        {
            pauseMenu.ToggleMenu();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.GetComponent<PseudoNPC>())
            characterDialogueSO = other.GetComponent<PseudoNPC>().GetDialogueSO();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited");
        characterDialogueSO = null;
        dialogueUI.CloseDialogueUI();
    }
}
