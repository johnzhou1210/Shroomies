using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerMovement : MonoBehaviour
{
    public EnemyStateManager StateManager;
    [SerializeField] float _moveSpeed, _changeDirectionChance;


    private void Start() {
        StateManager = GetComponent<EnemyStateManager>();
        if (Random.Range(0f,1f) <= .5f) {
            changeDirection();
        }
        StartCoroutine(randomDirectionChange());
    }

    // tulip moves side to side horizontally.
    private void FixedUpdate() {
        if (StateManager.CurrentState == StateManager.AliveState) {
            transform.position = transform.position + Vector3.right * Time.deltaTime * _moveSpeed;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.2f, 2.2f) , transform.position.y, 0);
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
