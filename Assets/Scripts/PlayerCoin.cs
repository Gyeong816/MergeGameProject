using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI coinText;
   private int _coinAmount;

   private void Awake()
   {
      coinText.text = _coinAmount.ToString();
   }

   public void AddCoin(int amount)
   {
      _coinAmount += amount;
      coinText.text = _coinAmount.ToString();
   }

   public void SubtractCoin(int amount)
   {
      _coinAmount -= amount;
      coinText.text = _coinAmount.ToString();
   }
}
