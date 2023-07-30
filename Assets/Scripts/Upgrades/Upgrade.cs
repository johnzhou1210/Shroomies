using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public Sprite Image;
    public string UpgradeName, UpgradeDescription;
    public bool Starter = true;
    public Upgrade[] UpgradesThatWillBeUnlocked;

}
