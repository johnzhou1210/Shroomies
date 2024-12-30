using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverText;

    private void OnEnable()
    {
        StageLogic.OnInvokeGameOver += SetGameOverVisibility;
    }

    private void OnDisable()
    {
        StageLogic.OnInvokeGameOver -= SetGameOverVisibility;
    }

    public void SetGameOverVisibility(bool val) {
        _gameOverText.SetActive(val);
    }
    
}
