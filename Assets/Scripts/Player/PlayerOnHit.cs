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
    bool dead = false, debounce = false;
    

    public void takeDamage(int damage) {
        ShroomieFormation formation = GameObject.FindWithTag("Shroomie Formation").GetComponent<ShroomieFormation>();
        if (debounce == false && !dead) {
            debounce = true;
            AudioManager.Instance.PlaySFX("Player Damage Sound");
            _shakeCam.Invoke(.08f, .1f);
            if (CurrentShroomies - damage < 0) { // killing blow
                dead = true;
                AudioManager.Instance.StopAllMusic();
                AudioManager.Instance.PlayMusic("Player Death Sound");
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
        debounce = false;
    }

    IEnumerator DestroyShroomie(GameObject obj) {
        Instantiate(_explosionPrefab, obj.transform.position, Quaternion.identity);
        obj.SetActive(false);
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
