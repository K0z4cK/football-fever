using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FootballerItem
{
    public string id;
    public string name;
    public Sprite sprite;
    public FootballerStats stats;
    public int points;
    public int price;

    public bool isUnique = false;
}
