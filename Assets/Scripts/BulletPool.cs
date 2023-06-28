using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType {
    NORMAL
}

public enum BulletOwnershipType {
    PLAYER,
    ALLY,
    ENEMY
}

public struct BulletInfo {
    public BulletInfo(int eyedee, BulletType typ, BulletOwnershipType ownshp, GameObject refrnce) {
        id = eyedee; type = typ; ownership = ownshp; reference = refrnce;
    }
    public int id;
    public BulletType type;
    public BulletOwnershipType ownership;
    public GameObject reference;
}


public class BulletPool : MonoBehaviour {
    public static BulletPool BulletPoolInstance;
    static int _numCreatedBullets = 0;

    [SerializeField] private GameObject[] _pooledBullets;

    List<BulletInfo> _bulletPool;

    private void Awake() {
        BulletPoolInstance = this;
    }

    // Start is called before the first frame update
    void Start() {
        _bulletPool = new List<BulletInfo>();
        // pregenerate bullets into pool
        foreach (GameObject bulletToPool in _pooledBullets) {
            expandPool(bulletToPool);
        }
    }

    void expandPool(GameObject pooledBullet) {
        for (int i = 0; i < 5; i++) {
            GameObject clone = Instantiate(pooledBullet, GameObject.FindWithTag("BulletPool").transform);
            Bullet currPooledBullet = clone.GetComponent<Bullet>();
            BulletInfo bulletInfo = new BulletInfo(++_numCreatedBullets, currPooledBullet.type, currPooledBullet.ownership, clone);
            _bulletPool.Add(bulletInfo);
            clone.SetActive(false);
        }
    }

    public BulletInfo GetBullet(BulletType bulletType, BulletOwnershipType bulletOwnership, float bulletVelocity, int bulletDamage) {
        BulletInfo activateBullet(BulletInfo bulletInfo) {
            bulletInfo.reference.SetActive(true);
            setVelocity(bulletInfo.reference.GetComponent<Bullet>(), bulletVelocity);
            setDamage(bulletInfo.reference.GetComponent<Bullet>(), bulletDamage);
            return bulletInfo;
        }

        // first try finding the bullet
        if (_bulletPool.Count > 0) {
            for (int i = 0; i < _bulletPool.Count; i++) {
                if (bulletType == _bulletPool[i].type && bulletOwnership == _bulletPool[i].ownership && !_bulletPool[i].reference.activeInHierarchy) {
                    return activateBullet(_bulletPool[i]);
                }
            }
        }
        // at this point, the bullet was not found in the pool, so create more.
        var objToClone = Array.Find(_pooledBullets, bullet => bullet.GetComponent<Bullet>().type == bulletType);
        expandPool(objToClone);
        // now try find again
        if (_bulletPool.Count > 0) {
            for (int i = 0; i < _bulletPool.Count; i++) {
                if (bulletType == _bulletPool[i].type && bulletOwnership == _bulletPool[i].ownership && !_bulletPool[i].reference.activeInHierarchy) {
                    return activateBullet(_bulletPool[i]);
                }
            }
        }
        return new BulletInfo(++_numCreatedBullets, bulletType, bulletOwnership, Array.Find(_pooledBullets, bullet => bullet.GetComponent<Bullet>().type == bulletType));
        
    }

   


    void setVelocity(Bullet bullet, float velocity) {
        bullet.velocity = velocity;
    }

    void setDamage(Bullet bullet, int damage) {
        bullet.damage = damage;
    }

}
