using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoNPC : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogueSo;

    public DialogueSO GetDialogueSO()
    {
        return dialogueSo;
    }
}
