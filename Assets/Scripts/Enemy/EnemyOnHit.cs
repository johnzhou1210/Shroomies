using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOnHit : MonoBehaviour, IDamageable
{
    public int MaxHealth = 5;
    int _currentHealth;
    public int _mulchReward;

    [SerializeField] UnityEvent onDeath;
    public UnityIntEvent giveMulch;

    Collider2D _hitbox;

    public void takeDamage(int damage) {
        if (isDead()) { return; }
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, MaxHealth);
        StopAllCoroutines();
        if (isDead()) {
            // disable collider
            _hitbox.enabled = false;
            onDeath.Invoke();
            giveMulch.Invoke(_mulchReward);
            AudioManager.Instance.PlaySFX("Enemy Death Sound");
            StartCoroutine(Flicker(3, .25f));
        } else {
            StartCoroutine(Flicker(1, .25f));
        }
        
    }

    public bool isDead() {
        return _currentHealth == 0;
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
        _hitbox = GetComponent<Collider2D>();
        setCurrHealthToMaxHealth();
    }

    public void setCurrHealthToMaxHealth() {
        _currentHealth = MaxHealth;
    }
}
