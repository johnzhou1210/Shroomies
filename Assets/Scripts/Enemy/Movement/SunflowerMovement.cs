using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerMovement : MonoBehaviour
{
    public EnemyStateManager StateManager;
    [SerializeField] float _moveSpeed, _changeDirectionChance;
    bool _changeDirectionDebounce = false;

    private void Start() {
        StateManager = GetComponent<EnemyStateManager>();
        if (Random.Range(0f,1f) <= .5f) {
            StartCoroutine(ChangeDirection());
        }
        StartCoroutine(randomDirectionChange());
    }

    // tulip moves side to side horizontally.
    private void FixedUpdate() {
        if (StateManager.CurrentState == StateManager.AliveState) {
            transform.position = transform.position + Vector3.right * Time.deltaTime * _moveSpeed;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.2f, 2.2f) , transform.position.y, 0);
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
