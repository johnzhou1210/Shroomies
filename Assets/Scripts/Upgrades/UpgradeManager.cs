using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    [Header("All Possible Upgrades")]
    [SerializeField] List<Upgrade> _upgrades;

    [Header("Debug")]
    public List<Upgrade> AvailableUpgrades;
    public List<Upgrade> ActiveUpgrades;
    [Space][Space]
    [Header("Rate of Fire Upgrades (Enter percent of reduction as decimal)")]
    [SerializeField] float _rateOfFireLevel1; [SerializeField] float _rateOfFireLevel2; [SerializeField] float _rateOfFireLevel3;
    [Header("Fire Power Upgrades (Enter percent increase as decimal)")]
    [SerializeField] float _firePowerLevel1; [SerializeField] float _firePowerLevel2; [SerializeField] float _firePowerLevel3;
    [Header("Critical Rate Upgrades (Enter new percentage as decimal)")]
    [SerializeField] float _criticalRateLevel1; [SerializeField] float _criticalRateLevel2; [SerializeField] float _criticalRateLevel3;
    [Header("Piercing Shot Upgrades (Enter number of targets to pierce)")]
    [SerializeField] int _piercingShotLevel1; [SerializeField] int _piercingShotLevel2; [SerializeField] int _piercingShotLevel3;

    private void Start() {
        AvailableUpgrades = new List<Upgrade>();
        foreach (Upgrade upgrade in _upgrades) {
            if (upgrade.Starter) {
                AvailableUpgrades.Add(upgrade);
            }
        }

        ActiveUpgrades = new List<Upgrade>();
    }

    public void OnUpgrade(Upgrade upgrade) {
        if (ActiveUpgrades.Find(upg => upg.UpgradeName.Equals(upgrade.UpgradeName)) == null) {
            ActiveUpgrades.Add(upgrade);
            AvailableUpgrades.Remove(upgrade);
            // special case for bouncing bullets to be added to pool: Player got the SpreadShot2 upgrade.
            if (upgrade.UpgradeName.Equals("Spread Shot II")) {
                AvailableUpgrades.Add(_upgrades.Find(upg => upg.UpgradeName.Equals("Ricochet")));
            }

            // note that it's the name, not the upgrade name.
            if (upgrade.name.Contains("1") || upgrade.name.Contains("2")) {
                // trim string part
                string left = upgrade.name.Substring(0, upgrade.name.Length - 1);
                int currUpgradeLevel = int.Parse(upgrade.name.Substring(upgrade.name.Length - 1));
                AvailableUpgrades.Add(_upgrades.Find(upg => upg.name.Equals(left + ((currUpgradeLevel + 1).ToString()))));
            }

        } else {
            // upgrade could not be found!
            Debug.LogError("The upgrade " + upgrade.UpgradeName + " could not be found!");
            
        }
    }

}
