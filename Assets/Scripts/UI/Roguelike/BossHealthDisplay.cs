using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthDisplay : MonoBehaviour
{
    [SerializeField] GameObject _healthBar, _healthBarFrame;
  
    public void OnHealthChange(int newHP, int maxHP) {
        _healthBar.transform.localScale = new Vector2((float)newHP / (float)maxHP, 1);
    }

    private void Start() {
        GameObject.FindWithTag("Roguelike Manager").GetComponent<StageLogic>().InvokeEnableBossHPDisplay.AddListener(SetBarEnabled);
    }

    public void SetBarEnabled(bool val) {
        _healthBarFrame.SetActive(val);
    }

}
