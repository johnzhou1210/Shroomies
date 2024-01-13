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
    //public GameObject enemyParticleExplosion;
    public ParticleSystem ParticlesExplosion;
    public ParticleSystem ParticlesBits;
    public int ParticlesDeath = 0;
    public int ParticlesAmount = 1;
    public int BitsAmount = 5;
    //public int ParticlesColor = 0;
    Material material;

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

            GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Flash", 0);

            //particle call here
            ParticleSystem.TextureSheetAnimationModule psDeathTSA = ParticlesExplosion.textureSheetAnimation;
            psDeathTSA.rowIndex = ParticlesDeath;

            ParticleSystem.MainModule psDeathMAIN = ParticlesExplosion.main;
            psDeathMAIN.startColor = ChangePalette.holder.color1;
            psDeathMAIN.maxParticles = ParticlesAmount;
            Instantiate(ParticlesExplosion,transform.position, Quaternion.identity);

            ParticleSystem.MainModule psBitsMAIN = ParticlesBits.main;
            psBitsMAIN.startColor = ChangePalette.holder.color3;
            psBitsMAIN.maxParticles = BitsAmount;
            Instantiate(ParticlesBits, transform.position, Quaternion.identity);

            psBitsMAIN.startColor = ChangePalette.holder.color1;
            psBitsMAIN.maxParticles = (int)Mathf.Floor((float)BitsAmount / 2f);
            Instantiate(ParticlesBits, transform.position, Quaternion.identity);

            //psTSA.rowIndex = 1;
            //psMAIN.startColor = ChangePalette.holder.color2;
            //psMAIN.maxParticles = 1;
            //Instantiate(ps, transform.position, Quaternion.identity);

            Camera.main.GetComponent<CameraShaker>().Shake(.01f, .1f);

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
            //SetColorOfAllEnabledSprites(Color.clear);
            //SetColorOfAllEnabledSprites(ChangePalette.holder.color2);

            //material.SetFloat("_Flash", 1);
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_ColorFlash", ChangePalette.holder.color2);
            GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Flash", 1);
            //SetColorOfAllEnabledSprites(ChangePalette.holder.color2);

            yield return new WaitForSeconds(flickerDelay / 1f);
            //SetColorOfAllEnabledSprites(Color.white);

            //material.SetFloat("_Flash", 0);
            GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Flash", 0);
            //SetColorOfAllEnabledSprites(Color.white);

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
