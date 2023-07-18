using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageInfo {
    public float Velocity, CritRate;
    public int Damage, PierceCount;
    public bool Bounce;

    public BulletDamageInfo(float vel, int dam, float crit, int pier, bool bounc) {
        Velocity = vel;
        Damage = dam;
        CritRate = crit;
        PierceCount = pier;
        Bounce = bounc;
    }

}

public class BasicBarrel : BarrelConfiguration
{
    public override void Fire(BulletType bulletType, BulletOwnershipType ownership, BulletDamageInfo dmgInfo) {


        AudioManager.Instance.PlaySFX( (ownership == BulletOwnershipType.PLAYER || ownership == BulletOwnershipType.ALLY) ? "Player Shoot Sound" : "Enemy Shoot Sound");
        
        foreach (Transform child in transform) {
            if (child != null) {
                BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(bulletType, ownership, dmgInfo);
                GameObject newBullet = newBulletInfo.Reference;
                newBullet.transform.position = child.position;
                newBullet.transform.rotation = child.rotation;
                newBullet.GetComponent<Bullet>().SetVelocity(child.up * dmgInfo.Velocity);
            }
        }
        
    }

   
}
