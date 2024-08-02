using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundsSO", menuName = "ScriptableObjects/Shop/BackgroundsSO")]
public class BackgroundsSO : ScriptableObject
{
    public List<BackgroundItem> backgroundItems;
}