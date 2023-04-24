using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameUIManager GameUIManager;
    [HideInInspector] public Character PlayerCharacter;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameUIManager.ToggleUIPause();
        }
        if(PlayerCharacter.CurrentState == Character.CharacterState.Dead)
        {
            _gameIsOver = true;
            GameOver();
        }
    }

    private void GameOver() => GameUIManager.ShowGameOverUI();

    public void GameIsFinished() => GameUIManager.ShowGameIsFinished();

    public void ReturnToTheMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
