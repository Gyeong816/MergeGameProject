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

    private void Awake()
    {
        inGameButton.onClick.AddListener(StartGame);
    }

    private void UpdateUI()
    {
        
    }

    private void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }
}
