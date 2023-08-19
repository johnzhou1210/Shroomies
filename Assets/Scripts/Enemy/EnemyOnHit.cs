using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOnHit : MonoBehaviour, IDamageable
{
    public int MaxHealth = 5;
    public int CurrentHealth;
    public int MulchReward;

    public UnityEvent OnDeath;
    public UnityIntEvent GiveMulch;

    public Collider2D Hitbox;

    public void takeDamage(int damage) {
        if (isDead()) { return; }
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        StopAllCoroutines();
        if (isDead()) {
            // disable collider
            Hitbox.enabled = false;
            OnDeath.Invoke();
            GiveMulch.Invoke(MulchReward);
            AudioManager.Instance.PlaySFX("Enemy Death Sound");
            StartCoroutine(Flicker(3, .24f));
        } else {
            StartCoroutine(Flicker(1, .12f));
        }
        
    }

    public bool isDead() {
        return CurrentHealth == 0;
    }

    IEnumerator Flicker(int amountOfTimes, float flickerDelay) {
        for (int i = 0; i < amountOfTimes; i++) {
            yield return new WaitForSeconds(flickerDelay / 2f);
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForSeconds(flickerDelay / 2f);
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (isDead()) {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.clear;
            gameObject.SetActive(false);
        }
        yield return null;
    }

    // Start is called before the first frame update
    void Start() {
        Hitbox = GetComponent<Collider2D>();
        setCurrHealthToMaxHealth();
    }

    public void setCurrHealthToMaxHealth() {
        
        CurrentHealth = MaxHealth;
    }
}
