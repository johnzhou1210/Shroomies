using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TulipMovement : MonoBehaviour
{
    public EnemyStateManager StateManager;
    [SerializeField] float _moveSpeed, _changeDirectionChance, _frequency, _amplitude;
    float sinCenterY;
    bool _changeDirectionDebounce = false;


    private void Start() {
        StateManager = GetComponent<EnemyStateManager>();
        sinCenterY = transform.parent.transform.position.y;
        if (Random.Range(0f,1f) <= .5f) {
            StartCoroutine(ChangeDirection());
        }
        StartCoroutine(randomDirectionChange());
    }

    // tulip moves side to side in a sine wave.
    private void FixedUpdate() {
        if (StateManager.CurrentState == StateManager.AliveState) {
            float sin;
            sinCenterY = transform.parent.transform.position.y;
            transform.position = transform.position + Vector3.right * Time.deltaTime * _moveSpeed;
            sin = Mathf.Sin(transform.position.x * _frequency) * _amplitude;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.2f, 2.2f) , sinCenterY + sin, 0);
            if (transform.position.x >= 2.1f || transform.position.x <= -2.1f) {
                StartCoroutine(ChangeDirection());
            }
        }
        
    }

    IEnumerator randomDirectionChange() {
        yield return new WaitUntil(() => StateManager.CurrentState == StateManager.AliveState);
        while (StateManager.CurrentState == StateManager.AliveState) {
            yield return new WaitForSeconds(1f);
            if (Random.Range(0f,1f) <= _changeDirectionChance) {
                StartCoroutine(ChangeDirection());   
            }
        }
    }



    IEnumerator ChangeDirection() {
        if (_changeDirectionDebounce == false) {
            _changeDirectionDebounce = true;
            _moveSpeed *= -1f;
            Debug.Log("Changed direction");
            yield return new WaitForSeconds(1f);
            _changeDirectionDebounce = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision == null) {
            StartCoroutine(ChangeDirection());
        }
    }


}
