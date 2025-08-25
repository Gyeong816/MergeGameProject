using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasketEnergy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private int energyAmount;
    [SerializeField] private int energyCost;
    
    private void Awake()
    {
        energyText.text = energyAmount.ToString();
    }

    public void UseEnergy()
    {
        energyAmount -= energyCost;
        energyText.text = energyAmount.ToString();
    }
}
