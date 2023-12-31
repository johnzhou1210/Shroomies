using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOnHit : MonoBehaviour, IDamageable
{
    public int MaxHealth = 5;
    public bool alreadyCounted = false;
    public int CurrentHealth;
    public int MulchReward;

    public Animator Animator;

    public UnityEvent OnDeath;
    [HideInInspector] public UnityIntEvent GiveMulch;

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
            StartCoroutine(Flicker(0, .35f));
            Animator.Play("Dead");
        } else {
            StartCoroutine(Flicker(1, .12f));
        }
        
    }

    public bool isDead() {
        return CurrentHealth == 0;
    }

    public void SetColorOfAllEnabledSprites(Color color) {
        foreach (Transform child in transform) {
            if (child.gameObject.activeInHierarchy && child.TryGetComponent(out SpriteRenderer rend)) {
                rend.color = color;
            }
        }
    }

    IEnumerator Flicker(int amountOfTimes, float flickerDelay) {
        for (int i = 0; i < amountOfTimes; i++) {
            yield return new WaitForSeconds(flickerDelay / 2f);
            SetColorOfAllEnabledSprites(Color.clear);
            yield return new WaitForSeconds(flickerDelay / 2f);
            SetColorOfAllEnabledSprites(Color.white);
        }
        if (isDead()) {
            yield return new WaitForSeconds(flickerDelay);
            SetColorOfAllEnabledSprites(Color.clear);
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
