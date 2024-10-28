using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class ShroomiePositionData {
    public Vector2 SpawnPosition;
}

public class ShroomieFormation : MonoBehaviour
{
    [SerializeField] ShroomiesFormationData FormationData;
    [SerializeField] GameObject _shroomiePrefab;

    public List<GameObject> ShroomieObjects;

    private void Start() {
        StartCoroutine(InitializeFormation());
        ShroomieObjects = new List<GameObject>();
    }

    IEnumerator InitializeFormation() {
        yield return new WaitUntil (() => FormationData != null && FormationData.Data != null && FormationData.Data.Count > 0);
        // spawn formation from given instructions.
        int spawned = 0;
        while (spawned < FormationData.Data.Count) {
            // create shroomie and place it.
            GameObject newShroomie = Instantiate(_shroomiePrefab, transform);
            Debug.Log("instantiated shroomie");
            // add listener to upgrade update event
            GetComponent<ShroomiesUpgradeController>().RequestShroomiesUpgradeUpdate.AddListener(newShroomie.GetComponent<ShroomieShooting>().OnUpgradeUpdate);
            Debug.Log("instantiated shroomie2");
            // also add listener for barrel respositioner for double shots
            GameObject.FindWithTag("UpgradeManager").GetComponent<UpgradeManager>().BulletTypeEvent.AddListener(newShroomie.transform.Find("BarrelConfigs").Find("BarrelLv1").GetComponent<DoubleShotBarrelWidthSpacing>().setSpacing);
            Debug.Log("instantiated shroomie3");
            Debug.Log(spawned);
            //Debug.Log("Added barrel listener");
            // find desired number
            ShroomiePositionData desiredShroomie = FormationData.Data[spawned];
            newShroomie.transform.localPosition = desiredShroomie.SpawnPosition;
            Debug.Log("instantiated shroomie4");
            spawned++;
            
            newShroomie.name = spawned.ToString();
            ShroomieObjects.Add(newShroomie);
            newShroomie.SetActive(false);
        }
    }

}

