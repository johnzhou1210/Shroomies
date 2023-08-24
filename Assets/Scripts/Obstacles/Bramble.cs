using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    [SerializeField] float _damageCooltime = 1f;

    bool _debounce = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && _debounce == false) {
            StartCoroutine(DoDebounce());
            collision.gameObject.GetComponent<PlayerOnHit>().takeDamage(_damage);
        }
    }

    IEnumerator DoDebounce() {
        _debounce = true;
        yield return new WaitForSeconds(_damageCooltime);
        _debounce = false;
    }

}
