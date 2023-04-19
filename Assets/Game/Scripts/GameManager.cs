using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character PlayerCharacter;
    private bool _gameIsOver;

    private void Awake()
    {
        PlayerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void Update()
    {
        if (_gameIsOver)
        {
            return;
        }
        if(PlayerCharacter.CurrentState == Character.CharacterState.Dead)
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
