using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DandelionOnHit : EnemyOnHit, IDamageable
{

    public new void takeDamage(int damage) {
        if (isDead()) { return; }
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        StopAllCoroutines();
        if (isDead()) {
            // disable collider
            Hitbox.enabled = false;
            OnDeath.Invoke();
            GiveMulch.Invoke(MulchReward);
            AudioManager.Instance.PlaySFX("Enemy Death Sound");
            //StartCoroutine(Flicker(3, .24f));
        } else {
            //StartCoroutine(Flicker(1, .12f));
        }
        
    }

    public void HideDeadBody() {
        SetColorOfAllEnabledSprites(Color.clear);
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() {
        Debug.Log("in dandelion on hit start");
        Debug.Log("currhealth is " + CurrentHealth + " and maxhealth is " + MaxHealth);
        Hitbox = GetComponent<Collider2D>();
        setCurrHealthToMaxHealth();
    }

}
