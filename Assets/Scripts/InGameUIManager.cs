using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
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
        energyText.text = GameManager.Instance.Player.stamina.ToString();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Lobby");
    }
}
