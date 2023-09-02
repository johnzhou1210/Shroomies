using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _gameOverText;
    public void OnGameOver(bool val) {
        _gameOverText.SetActive(val);
    }
}
