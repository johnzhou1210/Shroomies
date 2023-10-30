using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public Sprite Image;
    public string UpgradeName, UpgradeDescription, ShroomItUpDescription = "<color=\"red\">Same effect</color> is applied to Shroomies.";
    public List<string> descriptionKeyWords = new List<string>();
    public List<string> shroomItUpKeyWords = new List<string>();
    public bool Starter = true;
    public Upgrade[] UpgradesThatWillBeUnlocked;

}
