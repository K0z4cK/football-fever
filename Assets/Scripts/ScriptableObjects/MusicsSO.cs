using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicsSO", menuName = "ScriptableObjects/Shop/MusicsSO")]
public class MusicsSO : ScriptableObject
{
    public List<MusicItem> musicItems;
}