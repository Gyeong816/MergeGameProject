using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager  : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    public PlayerData Player { get; private set; }
    public event Action OnPlayerDataChanged;

    [Header("에너지 설정")]
    [SerializeField] private int maxEnergy = 100;           // 최대 에너지
    [SerializeField] private int energyCost = 5;            // 1회 소모량
    [SerializeField] private float recoveryInterval = 30f;  // 충전 주기(초)

    private float _energyTimer; // 회복 타이머

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
        Player.energy = maxEnergy; // 시작 시 풀 충전
    }

    private void Update()
    {
        HandleEnergyRecovery();
    }
    
    public void AddGold(int amount)
    {
        Player.gold += amount;
        OnPlayerDataChanged?.Invoke();
    }
    
    public bool CanUseEnergy()
    {
        return Player.energy >= energyCost;
    }

    public bool TryUseEnergy()
    {
        if (CanUseEnergy())
        {
            Player.energy -= energyCost;
            OnPlayerDataChanged?.Invoke();
            _energyTimer = 0f; // 사용 시 타이머 초기화
            return true;
        }
        return false;
    }

    private void HandleEnergyRecovery()
    {
        if (Player.energy >= maxEnergy) return;

        _energyTimer += Time.deltaTime;
        if (_energyTimer >= recoveryInterval)
        {
            RecoverEnergy(5); 
            _energyTimer = 0f;
        }
    }

    private void RecoverEnergy(int amount)
    {
        Player.energy += amount;
        if (Player.energy > maxEnergy)
            Player.energy = maxEnergy;

        OnPlayerDataChanged?.Invoke();
    }
    
    public bool TryUnlockFurniture(int cost)
    {
        if (Player.gold >= cost)
        {
            Player.gold -= cost;
            OnPlayerDataChanged?.Invoke();
            return true;
        }
        return false;
    }
    public float GetRemainingRecoveryTime()
    {
        if (Player.energy >= maxEnergy) return 0f; // 이미 풀충전
        return Mathf.Max(0, recoveryInterval - _energyTimer);
    }
    public int GetMaxEnergy() => maxEnergy;
    public int GetEnergyCost() => energyCost;
}
