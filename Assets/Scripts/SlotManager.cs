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
    
    [Header("가장자리 아이템 티어 설정")]
    [SerializeField] private int highTierRows = 2;
    [SerializeField] private int highTierColumns = 1;
    [SerializeField] private int middleTierRows = 2;
    [SerializeField] private int middleTierColumns = 1;
    
    [Header("아이템 스포너")]
    [SerializeField] ItemSpawner itemSpawner;
    
    private Slot[,] _slots;
    private List<ItemData> _allItems = new();
    
    public IEnumerable<Slot> AllSlots
    {
        get
        {
            foreach (var slot in _slots)
                yield return slot;
        }
    }
    
    private readonly Dictionary<SlotType, (int minTier, int maxTier)> _tierRanges = new()
    {
        { SlotType.HighTier, (5,6)},
        { SlotType.MiddleTier, (3,4)},
        { SlotType.LowTier, (1,2)},
    };

    private void Awake()
    {
        _slots = new Slot[columns, rows];
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
                
                
                _slots[x, y] = newSlot;
            }
        }
    }

    private void SpawnItems()
    {
        List<Slot> lowTierSlots = new List<Slot>();
        
        foreach (var slot in _slots)
        {
            var range = _tierRanges[slot.slotType];
            var candidateItems = ItemManager.Instance.GetItemsByTierRange(range.minTier, range.maxTier);
            if (candidateItems.Count == 0) continue;
            
            ItemData randomItemData = candidateItems[UnityEngine.Random.Range(0, candidateItems.Count)];
            
            switch (slot.slotType)
            {
                case SlotType.HighTier:
                    ItemManager.Instance.CreateItem(randomItemData, slot.transform, ItemState.Hidden);
                    break;
                case SlotType.MiddleTier:
                    ItemManager.Instance.CreateItem(randomItemData, slot.transform, ItemState.Disabled);
                    break;
                case SlotType.LowTier:
                    lowTierSlots.Add(slot);
                    ItemManager.Instance.CreateItem(randomItemData, slot.transform, ItemState.Active);
                    break;
            }
        }
        
       
        Slot target = lowTierSlots[UnityEngine.Random.Range(0, lowTierSlots.Count)];
        Item existingItem = target.GetComponentInChildren<Item>();
        Destroy(existingItem.gameObject);
        
        Instantiate(itemSpawner, target.transform);
    }

    public void UnlockHiddenItems(Slot slot)
    {
        int x = slot.gridX;
        int y = slot.gridY;
        
        UnlockItem(x-1, y);
        UnlockItem(x+1, y);
        UnlockItem(x, y+1);
        UnlockItem(x, y-1);
    }

    private void UnlockItem(int gridX, int gridY)
    {
        if (gridX < 0 || gridX >= _slots.GetLength(0) ||
            gridY < 0 || gridY >= _slots.GetLength(1))
            return;
        
        Slot targetSlot = _slots[gridX, gridY];
        Item item = targetSlot.GetComponentInChildren<Item>();
        
        if (item != null && item.State == ItemState.Hidden)
            item.SetItemState(ItemState.Disabled);
    }

    public Slot GetEmptySlot(int originX, int originY)
    {
        Slot nearestSlot = null;
        float nearestDist = float.MaxValue;
        
        foreach (var slot in _slots)
        {
            if (slot.transform.childCount == 0)
            {
                int disX = slot.gridX - originX;
                int disY = slot.gridY - originY;
                
                float dist = disX * disX + disY * disY;

                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestSlot = slot;
                }
            }
        }
        return nearestSlot;
    }
}
