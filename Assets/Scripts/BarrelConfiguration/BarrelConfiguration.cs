using UnityEngine;

public abstract class BarrelConfiguration : MonoBehaviour {
    public abstract void Fire(BulletType bulletType, BulletOwnershipType ownership, BulletDamageInfo dmgInfo);
}

