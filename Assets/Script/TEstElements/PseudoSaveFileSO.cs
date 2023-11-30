using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pseudo Save File")]
public class PseudoSaveFileSO : ScriptableObject
{
    public string CharacterName;
    public int CharacterLevel;

    public string CharacterCurrentZoneName;
    public string playTime;
}
