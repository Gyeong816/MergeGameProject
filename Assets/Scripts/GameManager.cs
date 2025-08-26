using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager  : MonoBehaviour
{
   public static GameManager  Instance { get; private set; }
   public PlayerData Player { get; private set; }
   public event Action OnPlayerDataChanged;
   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }
      Instance = this;
      DontDestroyOnLoad(gameObject);
      
      Player = new PlayerData();
   }
   
   public void AddGold(int amount)
   {
      Player.gold += amount;
      OnPlayerDataChanged?.Invoke();
   }

   public void UseStamina(int amount)
   {
      Player.stamina -= amount;
      OnPlayerDataChanged?.Invoke();
   }
}
