using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerOnHit : MonoBehaviour, IDamageable
{
    [SerializeField] Unity2FloatEvent _shakeCam;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] float _damageDelay = 1f;
    [SerializeField] UnityEvent _onPlayerDeath;
    public int CurrentShroomies = 0;
    public bool Debounce = false;
    public bool Dead = false;

    public Material mymat;
    public ParticleSystem ParticlesExplosion;

    public void takeDamage(int damage) {
        ShroomieFormation formation = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomieFormation>();
        if (Debounce == false && !Dead) {
            Debounce = true;
            AudioManager.Instance.PlaySFX("Player Damage Sound");
            _shakeCam.Invoke(.12f, .08f);
            if (CurrentShroomies - damage < 0) { // killing blow
                Dead = true;
                AudioManager.Instance.StopAllMusic(false);
                _onPlayerDeath.Invoke();
            } else {
                // inactivate shroomies based on damage
                for (int i = 0; i < damage; i++) {
                    // get shroomie at order CurrentShroomies
                    GameObject shroomieToDestroy = formation.transform.Find(CurrentShroomies.ToString()).gameObject;
                    StartCoroutine(DestroyShroomie(shroomieToDestroy));
                }
                CurrentShroomies = Mathf.Clamp(CurrentShroomies - damage, 0, 7);
            }
            // add period of invulnerability.
            StartCoroutine(RefreshDebounce(_damageDelay));
        }

    }

    IEnumerator RefreshDebounce(float duration) {
        if (Dead == false)
        {
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_ColorFlash", ChangePalette.holder.color1);
            GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Flash", 1);
            GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Outline", 1);

            Time.timeScale = 0.4f;

            yield return new WaitForSeconds(duration * 0.1f);
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_ColorFlash", ChangePalette.holder.color2);
            GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Flash", 1);

            yield return new WaitForSeconds(duration * 0.2f);
        }
        
        GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Flash", 0);

        if (Dead == false)
        {
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color1", ChangePalette.holder.color3);
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color4", ChangePalette.holder.color4);
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_ColorOutline", ChangePalette.holder.color1);
        }

        yield return new WaitForSeconds(duration * 0.1f);

        Time.timeScale = 1f;

        /*if (Dead == false)
        {
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color1", ChangePalette.holder.color2);
            GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color4", ChangePalette.holder.color1);
        }

        yield return new WaitForSeconds(duration * 0.2f);*/
        
        

        yield return new WaitForSeconds(duration * 0.8f);

        /*GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color1", ChangePalette.holder.color1);
        GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color2", ChangePalette.holder.color2);
        GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color3", ChangePalette.holder.color3);
        GetComponentInChildren<SpriteRenderer>().material.SetColor("_Color4", ChangePalette.holder.color4);
        GetComponentInChildren<SpriteRenderer>().material.SetColor("_ColorOutline", ChangePalette.holder.color4);*/
        GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Outline", 0);

        GetComponentInChildren<SpriteRenderer>().material = mymat;

        Debounce = false;
    }

    IEnumerator DestroyShroomie(GameObject obj) {
        //Instantiate(_explosionPrefab, obj.transform.position, Quaternion.identity);

        //particles
        ParticleSystem.TextureSheetAnimationModule psDeathTSA = ParticlesExplosion.textureSheetAnimation;
        psDeathTSA.rowIndex = 0;

        ParticleSystem.MainModule psDeathMAIN = ParticlesExplosion.main;
        psDeathMAIN.startColor = ChangePalette.holder.color1;
        psDeathMAIN.maxParticles = 4;

        Vector3 psDeathOffset = obj.transform.position;
        psDeathOffset.y -= 0.25f;
        Instantiate(ParticlesExplosion, psDeathOffset, Quaternion.identity);

        obj.GetComponent<Animator>().Play("ShroomieDeath");
        yield return new WaitForSeconds(.35f);
        obj.SetActive(false);
        yield return null;
    }

 
}
