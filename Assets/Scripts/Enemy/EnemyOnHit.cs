using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOnHit : MonoBehaviour, IDamageable
{
    public int MaxHealth = 5;
    public int CurrentHealth;
    [SerializeField] UnityEvent onDeath;

    Collider2D _hitbox;

    public void takeDamage(int damage) {
        if (CurrentHealth == 0) { return; }
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        StopAllCoroutines();
        if (CurrentHealth == 0) {
            // disable collider
            _hitbox.enabled = false;
            onDeath.Invoke();
            AudioManager.Instance.PlaySFX("Enemy Death Sound");
            StartCoroutine(Flicker(3, .25f));
        } else {
            StartCoroutine(Flicker(1, .25f));
        }
        
    }

    IEnumerator Flicker(int amountOfTimes, float flickerDelay) {
        for (int i = 0; i < amountOfTimes; i++) {
            yield return new WaitForSeconds(flickerDelay / 2f);
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForSeconds(flickerDelay / 2f);
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
        }
        if (CurrentHealth == 0) {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.clear;
            gameObject.SetActive(false);
        }
        yield return null;
    }

    // Start is called before the first frame update
    void Start() {
        _hitbox = GetComponent<Collider2D>();
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update() {

    }
}
