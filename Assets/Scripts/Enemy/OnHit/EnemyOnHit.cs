using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyOnHit : MonoBehaviour, IDamageable {
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
    public Material mymat;

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
            psDeathMAIN.startColor = ChangePalette.Holder.color1;
            psDeathMAIN.maxParticles = ParticlesAmount;
            Vector3 psDeathOffset = GetComponent<Transform>().position;
            psDeathOffset.y += 0.1f;
            Instantiate(ParticlesExplosion, psDeathOffset, Quaternion.identity);

            ParticleSystem.MainModule psBitsMAIN = ParticlesBits.main;
            psBitsMAIN.startColor = ChangePalette.Holder.color3;
            psBitsMAIN.maxParticles = BitsAmount;
            Vector3 psBitsOffset = GetComponent<Transform>().position;
            psBitsOffset.y += 0.25f;
            Instantiate(ParticlesBits, psBitsOffset, Quaternion.identity);

            psBitsMAIN.startColor = ChangePalette.Holder.color1;
            psBitsMAIN.maxParticles = (int)Mathf.Floor((float)BitsAmount / 2f);
            Instantiate(ParticlesBits, psBitsOffset, Quaternion.identity);

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

    protected void SetDescendantsMaterialFloat(string name, float value) {
        foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>()) {
            rend.material.SetFloat(name, value);
        }
    }

    protected void SetDescendantsMaterialColor(string name, Color value) {
        foreach (SpriteRenderer rend in GetComponentsInChildren<SpriteRenderer>()) {
            rend.material.SetColor(name, value);
        }
    }

    protected IEnumerator Flicker(int amountOfTimes, float flickerDelay) {
        for (int i = 0; i < amountOfTimes; i++) {
            yield return new WaitForSeconds(flickerDelay / 2f);

            SetDescendantsMaterialColor("_ColorFlash", ChangePalette.Holder.color2);

            SetDescendantsMaterialFloat("_Flash", 1);

            ParticleSystem.MainModule psMiniBitsMAIN = ParticlesBits.main;
            psMiniBitsMAIN.startColor = ChangePalette.Holder.color3;
            psMiniBitsMAIN.maxParticles = (int)Mathf.Floor((float)BitsAmount / 3f);
            Vector3 psBitsOffset = GetComponent<Transform>().position;
            psBitsOffset.y += 0.5f;
            Instantiate(ParticlesBits, psBitsOffset, Quaternion.identity);

            yield return new WaitForSeconds(flickerDelay / 2f);

            SetDescendantsMaterialFloat("_Flash", 0);

            GetComponentInChildren<SpriteRenderer>().material = mymat;

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
