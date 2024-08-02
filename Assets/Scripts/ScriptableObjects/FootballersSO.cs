using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootballersSO", menuName = "ScriptableObjects/Shop/FootballersSO")]
public class FootballersSO : ScriptableObject
{
    public List<FootballerItem> footballerItems;
}