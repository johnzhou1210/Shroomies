using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBarrel : BarrelConfiguration
{
    public override void Fire(BulletType bulletType, BulletOwnershipType ownership, float velocity, int damage) {


        AudioManager.Instance.PlaySFX( (ownership == BulletOwnershipType.PLAYER || ownership == BulletOwnershipType.ALLY) ? "Player Shoot Sound" : "Enemy Shoot Sound");
        
        foreach (Transform child in transform) {
            if (child != null) {
                BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(bulletType, ownership, velocity, damage);
                GameObject newBullet = newBulletInfo.Reference;
                newBullet.transform.position = child.position;
                newBullet.transform.rotation = child.rotation;
                newBullet.GetComponent<Bullet>().SetVelocity(child.up * velocity);
            }
        }
        
    }

   
}
