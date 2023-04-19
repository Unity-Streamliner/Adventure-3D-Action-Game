using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameManager GameManager;
    public TMPro.TextMeshProUGUI CoinText;
    public Slider HealthSlider;
    public GameObject UIPause;
    public GameObject UIGameOver;
    public GameObject UIGameIsFinished;

    private enum GameUIState {
        GamePlay, Pause, GameOver, GameIsFinished,
    }

    private GameUIState _currentState;

    void Update()
    {
        HealthSlider.value = GameManager.PlayerCharacter.GetComponent<Health>().CurrentHealthPercentage;
        CoinText.text = GameManager.PlayerCharacter.Coin.ToString();
    }

    private void SwitchUIState(GameUIState state)
    {
        
        HideAllGameUIStates();
        Time.timeScale = 1;

        switch(state)
        {
            case GameUIState.GamePlay:
                break;
            case GameUIState.Pause:
                Time.timeScale = 0;
                UIPause.SetActive(true);
                break;
            case GameUIState.GameOver:
                UIGameOver.SetActive(true);
                break;
            case GameUIState.GameIsFinished:
                UIGameIsFinished.SetActive(true);
                break;
        }
        _currentState = state;
    }

    private void HideAllGameUIStates()
    {
        UIPause.SetActive(false);
        UIGameOver.SetActive(false);
        UIGameIsFinished.SetActive(false);
    }

    public void ToggleUIPause()
    {
        if (_currentState == GameUIState.GamePlay)
        {
            SwitchUIState(GameUIState.Pause);
        } 
        else if (_currentState == GameUIState.Pause) 
        {
            SwitchUIState(GameUIState.GamePlay);
        }
    }
}
