using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClusterCollection : ScriptableObject
{
    public GameObject[] Clusters;
    public int WorldNumber, StageNumber;
}
