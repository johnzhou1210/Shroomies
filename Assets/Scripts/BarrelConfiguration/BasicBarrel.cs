using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageInfo {
    public float Velocity, CritRate;
    public int Damage, PierceCount, BulletClearLimit;
    public bool Bounce;
    public Transform Shooter;

    public BulletDamageInfo(float vel, int dam, float crit, int pier, bool bounc, int bulClrLmt) {
        Velocity = vel;
        Damage = dam;
        CritRate = crit;
        PierceCount = pier;
        Bounce = bounc;
        BulletClearLimit = bulClrLmt;
    }

    public void SetShooter(Transform shooter) {
        Shooter = shooter;
    }

}

public class BasicBarrel : BarrelConfiguration
{
    void excludeBulletPair(GameObject x, GameObject y) {
        Physics2D.IgnoreCollision(x.GetComponent<Collider2D>(), y.GetComponent<Collider2D>());
    }

    public override void Fire(BulletType bulletType, BulletOwnershipType ownership, BulletDamageInfo dmgInfo) {
        List<GameObject> createdBullets = new List<GameObject>();
        foreach (Transform child in transform) {
            if (child != null) {
                dmgInfo.SetShooter(transform.parent);
                BulletInfo newBulletInfo = BulletPool.BulletPoolInstance.GetBullet(bulletType, ownership, dmgInfo);
                GameObject newBullet = newBulletInfo.Reference;
                
                newBullet.transform.position = child.position;
                newBullet.transform.rotation = child.rotation;
                newBullet.GetComponent<Bullet>().SetVelocity(child.up * dmgInfo.Velocity);
                // add to ignore collision with other bullets of same shot
                createdBullets.Add(newBullet);
            }
        }

        // make all bullets ignore each other
        for (int i = 0; i < createdBullets.Count - 1; i++) {
            for (int j = i + 1; j < createdBullets.Count; j++) {
                excludeBulletPair(createdBullets[i], createdBullets[j]);
            }
        }

 
    }

   
}
