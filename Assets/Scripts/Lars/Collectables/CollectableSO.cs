using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Collectable", menuName = "Collectables", order = 1)]
public class CollectableSO : ScriptableObject
{
    public CollectableType collectable;
    public string collectableName;
}

