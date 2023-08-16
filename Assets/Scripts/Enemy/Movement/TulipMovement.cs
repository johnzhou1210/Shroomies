using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TulipMovement : MonoBehaviour
{
    public EnemyStateManager StateManager;
    [SerializeField] float _moveSpeed, _changeDirectionChance, _frequency, _amplitude;
    float sinCenterY;


    private void Start() {
        StateManager = GetComponent<EnemyStateManager>();
        sinCenterY = transform.parent.transform.position.y;
        if (Random.Range(0f,1f) <= .5f) {
            changeDirection();
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
            if (transform.position.x >= 2.2f || transform.position.x <= -2.2f) {
                changeDirection();
            }
        }
        
    }

    IEnumerator randomDirectionChange() {
        yield return new WaitUntil(() => StateManager.CurrentState == StateManager.AliveState);
        while (StateManager.CurrentState == StateManager.AliveState) {
            yield return new WaitForSeconds(1f);
            if (Random.Range(0f,1f) <= _changeDirectionChance) {
                changeDirection();   
            }
        }
    }



    void changeDirection() {
            _moveSpeed *= -1f;
            Debug.Log("Changed direction");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision == null) {
            changeDirection();
        }
    }


}
