using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerOnHit : MonoBehaviour, IDamageable
{
    [SerializeField] Unity2FloatEvent _shakeCam;
    public int MaxHealth = 5;
    public int CurrentHealth;

    public void takeDamage(int damage) {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        AudioManager.Instance.PlaySFX("Player Damage Sound");
        _shakeCam.Invoke(.08f, .1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
