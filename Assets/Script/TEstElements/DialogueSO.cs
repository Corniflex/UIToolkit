using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;

    public DialogueSystem.Dialogue dialogue;
}
