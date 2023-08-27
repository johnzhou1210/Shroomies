using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SnailBossOnHit : EnemyOnHit, IDamageable
{
   [HideInInspector]  public Unity2IntEvent InvokeHealthBarUpdate;
    [SerializeField] GameObject explosionPrefab;

    public new void takeDamage(int damage) {
        if (isDead()) { return; }
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        InvokeHealthBarUpdate.Invoke(CurrentHealth, MaxHealth);
        StopAllCoroutines();
        if (isDead()) {
            // disable collider
            Hitbox.enabled = false;
            OnDeath.Invoke();
            GiveMulch.Invoke(MulchReward);
            AudioManager.Instance.StopAllMusic(false);
            AudioManager.Instance.PlaySFX("Snail Boss Death");
            GetComponent<Animator>().Play("ShroomSnailDeath");
            StartCoroutine(ExplosionEffect(64, 1.4f));
            AudioManager.Instance.PlaySFX("Enemy Death Sound");
            StartCoroutine(Flicker(3, .24f));
            Invoke("HideDeadBody", 7f);
        } else {
            StartCoroutine(Flicker(1, .12f));
        }
        
    }

    IEnumerator Flicker(int amountOfTimes, float flickerDelay) {
        for (int i = 0; i < amountOfTimes; i++) {
            yield return new WaitForSeconds(flickerDelay / 2f);
            SetColorOfAllEnabledSprites(Color.clear);
            yield return new WaitForSeconds(flickerDelay / 2f);
            SetColorOfAllEnabledSprites(Color.white);
        } 
        yield return null;
    }

    IEnumerator LowHealthExplosion() {
        yield return null;
    }


    public IEnumerator ExplosionEffect(int numTimes = 32, float radius = 1.4f) {
        float maxOffset = radius;
        float minOffset = -maxOffset;
        for (int i = 0; i < numTimes; i++) {
            Vector3 offsetVector = new Vector3(Random.Range(minOffset, maxOffset), Random.Range(minOffset, maxOffset), 0);
            GameObject explosion = Instantiate(explosionPrefab, transform.position + offsetVector , Quaternion.identity);
            explosion.transform.localScale *= Random.Range(2.8f, 4f);
            AudioManager.Instance.PlaySFX("Explosion Sound");
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    public void HideDeadBody() {
        SetColorOfAllEnabledSprites(Color.clear);
        gameObject.SetActive(false);
    }


    IEnumerator AttachHPBarListener() {
        yield return new WaitUntil(() => GameObject.FindWithTag("BossHealthBar") != null);
        InvokeHealthBarUpdate.AddListener(GameObject.FindWithTag("BossHealthBar").GetComponent<BossHealthDisplay>().OnHealthChange); // attach listener for boss health bar
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(AttachHPBarListener());
        Debug.Log("currhealth is " + CurrentHealth + " and maxhealth is " + MaxHealth);
        Hitbox = GetComponent<Collider2D>();
        setCurrHealthToMaxHealth();
    }

}
