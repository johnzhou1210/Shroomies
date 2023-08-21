using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum BulletType {
    NORMAL,
    WIDE1,
    WIDE2,
    WIDE3,
    NORMAL_S,
    WIDE1_S,
    WIDE2_S,
    WIDE3_S,
    DANDELION,
    ROSETHORN,
    SUNFLOWER,
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
    static int _numCreatedBullets = 0;

    [SerializeField] private GameObject[] _pooledBullets;

    List<BulletPoolEntry> _bulletPool;

    private void Awake() {
        BulletPoolInstance = this;
    }

    // Start is called before the first frame update
    void Start() {
        _bulletPool = new List<BulletPoolEntry>();
        // pregenerate bullets into pool
        foreach (GameObject bulletToPool in _pooledBullets) {
            // add entry first
            BulletPoolEntry newEntry = new BulletPoolEntry(bulletToPool.GetComponent<Bullet>().Type);
            _bulletPool.Add(newEntry);
            expandPool(newEntry, bulletToPool, 8);
        }
    }

    void expandPool(BulletPoolEntry desiredPool, GameObject pooledBullet, int bulletsToCreate) {
        // first get the number of these pooled bullets in the pool
        //Debug.Log("attempting to create " + bulletsToCreate + " bullets!");
        for (int i = 0; i < bulletsToCreate; i++) {
            GameObject clone = Instantiate(pooledBullet, GameObject.FindWithTag("BulletPool").transform);
            Bullet currPooledBullet = clone.GetComponent<Bullet>();
            BulletInfo bulletInfo = new BulletInfo(++_numCreatedBullets, currPooledBullet.Type, currPooledBullet.Ownership, clone);
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
            return bulletInfo;
        }
        BulletPoolEntry findDesiredPool(BulletType type) {
            foreach (BulletPoolEntry entry in _bulletPool) {
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
        var objToClone = Array.Find(_pooledBullets, bullet => bullet.GetComponent<Bullet>().Type == bulletType && bullet.GetComponent<Bullet>().Ownership == bulletOwnership);
        expandPool(desiredPoolEntry, objToClone, desiredPool.Count^2 );
        // now try find again
        if (desiredPool.Count > 0) {
            for (int i = 0; i < desiredPool.Count; i++) {
                if (bulletType == desiredPool[i].Type && bulletOwnership == desiredPool[i].Ownership && !desiredPool[i].Reference.activeInHierarchy) {
                    return activateBullet(desiredPool[i]);
                }
            }
        }
        Debug.LogError("Bullets getting produced too quickly to catch up with bullet pool!");
        return new BulletInfo(++_numCreatedBullets, bulletType, bulletOwnership, Array.Find(_pooledBullets, bullet => bullet.GetComponent<Bullet>().Type == bulletType && bullet.GetComponent<Bullet>().Ownership == bulletOwnership));
        
    }

   


    

}
