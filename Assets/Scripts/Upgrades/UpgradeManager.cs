using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour {

    [Header("All Possible Upgrades")]
    [SerializeField] List<Upgrade> _upgrades;

    [Header("Debug")]
    public List<Upgrade> AvailableUpgrades;
    public List<Upgrade> ActiveUpgrades;
    public List<Upgrade> ShroomItUpUpgrades;

    [Space][Space]

    [SerializeField] UnityFloatBoolEvent _critUpgrade, _firePowerUpgrade, _rateOfFireUpgrade;
    [SerializeField] UnityIntBoolEvent _extraShotUpgrade, _piercingShotUpgrade, _wideShotUpgrade;
    [SerializeField] Unity2BoolEvent _ricochetUpgrade;

    public UnityBulletTypeEvent BulletTypeEvent;

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
        ShroomItUpUpgrades = new List<Upgrade>();
    }

    public void OnUpgrade(Upgrade upgrade, bool shroomItUp) {
        if (ActiveUpgrades.Find(upg => upg.UpgradeName.Equals(upgrade.UpgradeName)) == null) {
            ActiveUpgrades.Add(upgrade);
            if (shroomItUp) {
                ShroomItUpUpgrades.Add(upgrade);
            }
            AvailableUpgrades.Remove(upgrade);
            // dynamically add upgrades to pool that will be unlocked
            foreach (Upgrade newEntry in upgrade.UpgradesThatWillBeUnlocked) {
                if (AvailableUpgrades.Find(upg => upg.UpgradeName == newEntry.UpgradeName) == null) {
                    AvailableUpgrades.Add(newEntry);
                }
            }
            // apply upgrade
            applyUpgrade(upgrade, shroomItUp);
            // send event to upgrade shroomies
            if (shroomItUp) {
                Debug.Log("Invoking shroomie upgrade update");
                GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomiesUpgradeController>().RequestShroomiesUpgradeUpdate.Invoke();
            }
        } else {
            // upgrade could not be found!
            Debug.LogError("The upgrade " + upgrade.UpgradeName + " could not be found!");

        }
    }

    void applyUpgrade(Upgrade upgrade, bool shroomItUp) {
        // trim string part
        
        int right;
        bool hasNumber = int.TryParse(upgrade.name.Substring(upgrade.name.Length - 1), out right);
        string left = hasNumber ? upgrade.name.Substring(0, upgrade.name.Length - 1) : upgrade.name;

        Debug.Log("Left is " + left + ", Right is " + right);
        switch (left) {
            /* Critical Hit Upgrades */
            case "Critical":
                Debug.Log("upgraded critical");
                switch (right) { case 1: _critUpgrade.Invoke(_criticalRateLevel1, shroomItUp); break; case 2: _critUpgrade.Invoke(_criticalRateLevel2, shroomItUp); break; case 3: _critUpgrade.Invoke(_criticalRateLevel3, shroomItUp); break; }
                break;
            /* Fire Power Upgrades */
            case "FirePower":
                Debug.Log("upgraded firepower");
                switch (right) { case 1: _firePowerUpgrade.Invoke(_firePowerLevel1, shroomItUp); break; case 2: _firePowerUpgrade.Invoke(_firePowerLevel2, shroomItUp); break; case 3: _firePowerUpgrade.Invoke(_firePowerLevel3, shroomItUp); break; }
                break;
            /* Rate of Fire Upgrades */
            case "RateOfFire":
                Debug.Log("upgraded rateoffire");
                switch (right) { case 1: _rateOfFireUpgrade.Invoke(_rateOfFireLevel1, shroomItUp); break; case 2: _rateOfFireUpgrade.Invoke(_rateOfFireLevel2, shroomItUp); break; case 3: _rateOfFireUpgrade.Invoke(_rateOfFireLevel3, shroomItUp); break; }
                break;
            /* Piercing Shot Upgrades */
            case "PiercingShot":
                Debug.Log("upgraded piercingshot");
                switch (right) { case 1: _piercingShotUpgrade.Invoke(_piercingShotLevel1, shroomItUp); break; case 2: _piercingShotUpgrade.Invoke(_piercingShotLevel2, shroomItUp); break; case 3: _piercingShotUpgrade.Invoke(_piercingShotLevel3, shroomItUp); break; }
                break;


            /* Extra Shot Upgrades */
            case "ExtraShot":
                Debug.Log("upgraded extrashot");
                _extraShotUpgrade.Invoke(right, shroomItUp);
                BulletTypeEvent.Invoke(GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerShooting>().CurrentBulletType);
                break;
            /* Wide Shot Upgrades */
            case "WideShot":
                Debug.Log("upgraded wideshot");
                _wideShotUpgrade.Invoke(right, shroomItUp);
                BulletTypeEvent.Invoke(GameObject.FindWithTag("Player").gameObject.GetComponent<PlayerShooting>().CurrentBulletType);
                break;


            /* Ricochet Upgrade */
            case "Ricochet":
                Debug.Log("upgraded ricochet");
                _ricochetUpgrade.Invoke(true, shroomItUp); break;


        }
    }


}
