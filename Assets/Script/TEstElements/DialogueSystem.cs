using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem
{
    [Serializable]
    public class Dialogue
    {
        public string text;
        public bool hasChoice;

        public Choice choice;

        public bool isLastDialogue;
    }

    [Serializable]
    public class Choice
    {
        public string[] options;
        public Dialogue[] choiceFollow;
    }
}
