using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlotManager : MonoBehaviour
{
    [Header("Panel 크기")] 
    [SerializeField] private int rows = 11;
    [SerializeField] private int columns = 7;
    
    [Header("프리펩 & 파넬")] 
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Transform gridPanel;
    [SerializeField] private Item itemPrefab;
    
    [Header("가장자리 아이템 티어 설정 ")]
    [SerializeField] private int highTierRows = 2;
    [SerializeField] private int highTierColumns = 1;

    [SerializeField] private int middleTierRows = 2;
    [SerializeField] private int middleTierColumns = 1;
    
    private Slot[,] _slots;
    private List<ItemData> _allItems = new();
    
    private readonly Dictionary<SlotType, (int minTier, int maxTier)> _tierRanges = new()
    {
        { SlotType.HighTier, (5,6)},
        { SlotType.MiddleTier, (3,4)},
        { SlotType.LowTier, (1,2)},
    };

    private void Awake()
    {
        _slots = new Slot[rows, columns];
        CreateSlots();
    }

    private void Start()
    {
        ItemManager.Instance.OnItemsLoaded += SpawnItems;
    }

    private void CreateSlots()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Slot newSlot = Instantiate(slotPrefab, gridPanel);
                newSlot.gridX = x;
                newSlot.gridY = y;
                
                bool isHighTierZone =
                    (x < highTierColumns || x >= columns - highTierColumns) || 
                    (y < highTierRows    || y >= rows - highTierRows);
                
                bool isMiddleTierZone =
                    !isHighTierZone && (
                        (x < highTierColumns + middleTierColumns || x >= columns - (highTierColumns + middleTierColumns)) ||
                        (y < highTierRows + middleTierRows       || y >= rows - (highTierRows + middleTierRows))
                    );
                
                if (isHighTierZone)
                    newSlot.slotType = SlotType.HighTier;
                else if (isMiddleTierZone)
                    newSlot.slotType = SlotType.MiddleTier;
                else
                    newSlot.slotType = SlotType.LowTier;
                
                
                _slots[y, x] = newSlot;
            }
        }
    }

    private void SpawnItems()
    {
        foreach (var slot in _slots)
        {
            var range = _tierRanges[slot.slotType];
            var candidateItems = ItemManager.Instance.GetItemsByTierRange(range.minTier, range.maxTier);
            if (candidateItems.Count == 0) continue;
            
            ItemData randomItemData = candidateItems[UnityEngine.Random.Range(0, candidateItems.Count)];
            ItemManager.Instance.CreateItem(randomItemData, slot.transform);

        }
    }
   
}
