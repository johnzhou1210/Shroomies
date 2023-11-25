using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ClusterTotalMulch : MonoBehaviour
{
    [SerializeField] public float totalMulch;
    [SerializeField] public bool reset;
    [SerializeField] public float childCount;

    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in transform) {
            if(child.gameObject.tag == "Enemy" && !child.gameObject.GetComponent<EnemyOnHit>().alreadyCounted) {
                totalMulch += child.gameObject.GetComponent<EnemyOnHit>().MulchReward;
                child.gameObject.GetComponent<EnemyOnHit>().alreadyCounted = true;
            }
        }

        if(childCount != transform.childCount)
            reset = true;

        if(reset) {
            totalMulch = 0f;
            foreach (Transform child in transform) {
                if (child.gameObject.tag == "Enemy" && child.gameObject.GetComponent<EnemyOnHit>().alreadyCounted) {
                    child.gameObject.GetComponent<EnemyOnHit>().alreadyCounted = false;
                }
            }
            reset = false;
            childCount = transform.childCount;
        }
    }

}
