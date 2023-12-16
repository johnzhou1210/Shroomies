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
        yield return new WaitForSeconds(duration);
        Debounce = false;
    }

    IEnumerator DestroyShroomie(GameObject obj) {
        Instantiate(_explosionPrefab, obj.transform.position, Quaternion.identity);
        obj.GetComponent<Animator>().Play("ShroomieDeath");
        yield return new WaitForSeconds(.35f);
        obj.SetActive(false);
        yield return null;
    }

 
}
