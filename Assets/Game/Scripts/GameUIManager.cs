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

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = GameManager.PlayerCharacter.GetComponent<Health>().CurrentHealthPercentage;
        CoinText.text = GameManager.PlayerCharacter.Coin.ToString();
    }
}
