using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasketEnergy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private int energyAmount;       
    [SerializeField] private int energyCost;         
    [SerializeField] private int maxEnergy = 100;     
    [SerializeField] private float recoveryInterval = 30f;
    
    private Coroutine recoveryCoroutine; 
    private void Awake()
    {
        UpdateEnergyUI();
    }

    public void UseEnergy()
    {
        energyAmount -= energyCost;
        UpdateEnergyUI();
        if (recoveryCoroutine == null)
        {
            recoveryCoroutine = StartCoroutine(EnergyRecoveryRoutine());
        }
    }

    public bool CanUseEnergy()
    {
        return energyAmount >= energyCost;
    }

    private void UpdateEnergyUI()
    {
        energyText.text = energyAmount.ToString();
    }

    private IEnumerator EnergyRecoveryRoutine()
    {
        while (true)
        {
            if (energyAmount >= maxEnergy)
            {
                recoveryCoroutine = null;
                timerObject.SetActive(false);
                yield break;
            }

            float timer = recoveryInterval;
            
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                timerObject.SetActive(true);
                timerText.text = Mathf.Ceil(timer).ToString(); 
                yield return null; 
            }
            
            energyAmount += energyCost;
            if (energyAmount > maxEnergy)
                energyAmount = maxEnergy;

            UpdateEnergyUI();
        }
    }

  
}
