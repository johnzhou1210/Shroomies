using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class ShroomiePositionData {
  
    public Vector2 SpawnPosition;
    public int order;
   
}

public class ShroomieFormation : MonoBehaviour
{
    [SerializeField] List<ShroomiePositionData> _formationData;
    [SerializeField] GameObject _shroomiePrefab;

    private void Start() {
        // spawn formation from given instructions.
        int spawned = 0;
        while (spawned < _formationData.Count) {
            // find desired number
            ShroomiePositionData desiredShroomie = _formationData.Find(shrm => shrm.order == spawned + 1);
            // create shroomie and place it.
            GameObject newShroomie = Instantiate(_shroomiePrefab, transform);
            newShroomie.transform.localPosition = desiredShroomie.SpawnPosition;
            spawned++;
        }
    }

}

