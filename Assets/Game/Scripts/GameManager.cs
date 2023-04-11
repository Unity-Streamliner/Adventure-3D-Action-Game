using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Character _playerCharacter;
    private bool _gameIsOver;

    private void Awake()
    {
        _playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void Update()
    {
        if (_gameIsOver)
        {
            return;
        }
        if(_playerCharacter.CurrentState == Character.CharacterState.Dead)
        {
            _gameIsOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
    }

    public void GameIsFinished()
    {
        Debug.Log("GAME IS FINISHED");
    }
}
