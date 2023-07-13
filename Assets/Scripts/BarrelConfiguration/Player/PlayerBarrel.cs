using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarrel : BarrelConfiguration
{
    public override void Fire() {
        AudioManager.Instance.PlaySFX("Player Shoot Sound");
        //Debug.Log(BulletPool.BulletPoolInstance);
        foreach (Transform child in transform) {
            if (child != null) {
                BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(BulletType.NORMAL, BulletOwnershipType.PLAYER, 5f, 1);
                GameObject newBullet = newBulletInfo.Reference;
                newBullet.transform.position = child.position;
                newBullet.transform.rotation = child.rotation;
                newBullet.GetComponent<Bullet>().SetMoveDirection(Vector2.up);
            }
        }
        
    }

   
}
