using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI coinText;
   [SerializeField] private int coinAmount;

   private void Awake()
   {
      coinText.text = coinAmount.ToString();
   }

   public void AddCoin(int amount)
   {
      coinAmount += amount;
      coinText.text = coinAmount.ToString();
   }

   public void SubtractCoin(int amount)
   {
      coinAmount -= amount;
      coinText.text = coinAmount.ToString();
   }
}
