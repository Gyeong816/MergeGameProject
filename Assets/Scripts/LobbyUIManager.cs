using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button inGameButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI energyTimerText;

    private void Awake()
    {
        inGameButton.onClick.AddListener(StartGame);
    }
    
    private void Update()
    {
        UpdateEnergyTimerUI();
    }

    private void UpdateEnergyTimerUI()
    {
        float remain = GameManager.Instance.GetRemainingRecoveryTime();
    
        if (GameManager.Instance.Player.energy >= GameManager.Instance.GetMaxEnergy())
        {
            energyTimerText.text = "FULL";
        }
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(remain);
            energyTimerText.text = $"{time.Seconds:D2}";
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.OnPlayerDataChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerDataChanged -= UpdateUI;
    }
    
    private void UpdateUI()
    {
        coinText.text = GameManager.Instance.Player.gold.ToString();
        energyText.text = GameManager.Instance.Player.energy.ToString();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }
}
