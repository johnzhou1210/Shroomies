using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum BulletType {
    NORMAL = 0, NORMAL_S = 4,
    WIDE1 = 1, WIDE2 = 2, WIDE3 = 3, WIDE1_S = 5, WIDE2_S = 6, WIDE3_S = 7,
    DANDELION = 8,
    ROSETHORN = 9,
    SUNFLOWER = 10,
    SNAIL_BOSS_WIDE = 11, SNAIL_BOSS_BELCH = 12
}

public enum BulletOwnershipType {
    PLAYER,
    ALLY,
    ENEMY,
}

public class BulletPoolEntry {
    public BulletPoolEntry() {
        // blank
    }
    public BulletPoolEntry(BulletType type) {
        Pool = new List<BulletInfo>();
        WhatKindOfBullets = type;
    }
    public void AddBulletToEntryPool(BulletInfo bullet) {
        Pool.Add(bullet);
    }
    public List<BulletInfo> Pool;
    public BulletType WhatKindOfBullets;
}

public class BulletInfo {
    public BulletInfo(int eyedee, BulletType typ, BulletOwnershipType ownshp, GameObject refrnce) {
        Id = eyedee; Type = typ; Ownership = ownshp; Reference = refrnce;
    }
    public int Id;
    public BulletType Type;
    public BulletOwnershipType Ownership;
    public GameObject Reference;
}


public class BulletPool : MonoBehaviour {
    public static BulletPool BulletPoolInstance;

    private GameObject[] pooledBullets;
    private static int numCreatedBullets = 0;
    private List<BulletPoolEntry> bulletPool;

    private string bulletPrefabsPath = "Prefabs/Bullets";

    private void Awake() {
        BulletPoolInstance = this;
        // fill in pooled bullets from project assets
        pooledBullets = Resources.LoadAll<GameObject>(bulletPrefabsPath);
    }

    // Start is called before the first frame update
    private void Start() {
        bulletPool = new List<BulletPoolEntry>();
        // pregenerate bullets into pool
        foreach (GameObject bulletToPool in pooledBullets) {
            // add entry first
            BulletPoolEntry newEntry = new BulletPoolEntry(bulletToPool.GetComponent<Bullet>().Type);
            bulletPool.Add(newEntry);
            ExpandPool(newEntry, bulletToPool, 8);
        }
    }

    private void ExpandPool(BulletPoolEntry desiredPool, GameObject pooledBullet, int bulletsToCreate) {
        // first get the number of these pooled bullets in the pool
        for (int i = 0; i < bulletsToCreate; i++) {
            GameObject clone = Instantiate(pooledBullet, GameObject.FindWithTag("BulletPool").transform);
            Bullet currPooledBullet = clone.GetComponent<Bullet>();
            BulletInfo bulletInfo = new BulletInfo(++numCreatedBullets, currPooledBullet.Type, currPooledBullet.Ownership, clone);
            desiredPool.AddBulletToEntryPool(bulletInfo);
            clone.SetActive(false);
        }
    }

    public BulletInfo GetBullet(BulletType bulletType, BulletOwnershipType bulletOwnership, BulletDamageInfo dmgInfo) {
        BulletInfo activateBullet(BulletInfo bulletInfo) {
            bulletInfo.Reference.SetActive(true);
            Bullet bulletComponent = bulletInfo.Reference.GetComponent<Bullet>();
            bulletComponent.SetVelocity(dmgInfo.Velocity);
            bulletComponent.SetDamage(dmgInfo.Damage);
            bulletComponent.SetCritRate(dmgInfo.CritRate);
            bulletComponent.SetPierceCount(dmgInfo.PierceCount);
            bulletComponent.SetBounce(dmgInfo.Bounce);
            bulletComponent.SetBulletClearLimit(dmgInfo.BulletClearLimit);
            bulletComponent.SetShooter(dmgInfo.Shooter);
            return bulletInfo;
        }
        BulletPoolEntry findDesiredPool(BulletType type) {
            foreach (BulletPoolEntry entry in bulletPool) {
                if (type == entry.WhatKindOfBullets) {
                    return entry;
                }
            }
            Debug.LogError("Bullet pool of desired type not found!");
            return new BulletPoolEntry();
        }

        // first find the pool of the desired bulletType
        BulletPoolEntry desiredPoolEntry = findDesiredPool(bulletType);
        var desiredPool = desiredPoolEntry.Pool;

        // then try finding an inactive bullet
        if (desiredPool.Count > 0) {
            for (int i = 0; i < desiredPool.Count; i++) {
                if (bulletType == desiredPool[i].Type && bulletOwnership == desiredPool[i].Ownership && !desiredPool[i].Reference.activeInHierarchy) {
                    return activateBullet(desiredPool[i]);
                }
            }
        }
        // at this point, the bullet was not found in the pool, so create more.
        var objToClone = Array.Find(pooledBullets, bullet => bullet.GetComponent<Bullet>().Type == bulletType && bullet.GetComponent<Bullet>().Ownership == bulletOwnership);
        ExpandPool(desiredPoolEntry, objToClone, desiredPool.Count^2 );
        // now try find again
        if (desiredPool.Count > 0) {
            for (int i = 0; i < desiredPool.Count; i++) {
                if (bulletType == desiredPool[i].Type && bulletOwnership == desiredPool[i].Ownership && !desiredPool[i].Reference.activeInHierarchy) {
                    return activateBullet(desiredPool[i]);
                }
            }
        }
        Debug.LogError("Bullets getting produced too quickly to catch up with bullet pool!");
        return new BulletInfo(++numCreatedBullets, bulletType, bulletOwnership, Array.Find(pooledBullets, bullet => bullet.GetComponent<Bullet>().Type == bulletType && bullet.GetComponent<Bullet>().Ownership == bulletOwnership));
        
    }

   


    

}
