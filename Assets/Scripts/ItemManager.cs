using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public event Action OnItemsLoaded;
    public event Action OnItemMerged;
    public bool IsLoaded { get; private set; }
    
    [SerializeField] private Item itemPrefab;
    [SerializeField] private float itemMoveSpeed = 0.2f;
    [SerializeField] private SlotManager slotManager;
    [SerializeField] private GameObject bonusItem;
    
    private List<ItemData> _allItems = new();
    private List<ItemData> _activeItems = new();
    private Dictionary<string, Sprite> _iconDict = new();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _ = LoadItemDataAsync();
    }
    
    private async Task LoadItemDataAsync()
    {
        try
        {
            _allItems = await TsvLoader.LoadTableAsync<ItemData>("Items");

            foreach (var item in _allItems)
            {
                if (!_iconDict.ContainsKey(item.Icon))
                {
                    var handle = Addressables.LoadAssetAsync<Sprite>(item.Icon);
                    await handle.Task;
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        _iconDict[item.Icon] = handle.Result;
                    }
                }
            }
            IsLoaded = true;
            OnItemsLoaded?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[LoadItemDataAsync] 로드 중 에러: {ex}");
        }
    }

    public Sprite GetIcon(string key)
    {
        if (_iconDict.TryGetValue(key, out var sprite))
            return sprite;

        Debug.LogWarning($"{key} 없음");
        return null;
    }
    
    public Item CreateItem(ItemData data, Transform parent, ItemState state)
    {
        Item newItem = Instantiate(itemPrefab, parent);
        newItem.ItemData = data;
        newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        if (_iconDict.TryGetValue(data.Icon, out var sprite))
            newItem.GetComponent<Image>().sprite = sprite;
        else
            Debug.LogWarning($"{data.Icon} 없음");

        newItem.SetItemState(state);
        
        if (state == ItemState.Active)
            _activeItems.Add(data);
        
        return newItem;
    }

    public List<string> GetActiveItemsNames()
    {
        List<string> activeItemName = new();
        foreach (var item in _activeItems)
        {
            activeItemName.Add(item.Name);
        }
     
        return activeItemName;
    }

    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < _activeItems.Count; i++)
        {
            if (_activeItems[i].Name == itemName)
            {
                _activeItems.RemoveAt(i);
                return;
            }
        }
        Debug.LogWarning($"{itemName}을 찾지 못했습니다.");
    }
    
    public void CreateRandomItem(Slot orignalSlot)
    {
        Slot emptySlot = slotManager.GetEmptySlot(orignalSlot.gridX, orignalSlot.gridY);
        if (emptySlot == null)
            return;
        int randomChance = UnityEngine.Random.Range(0, 10);
        
        int targetTier = 1;

        if (randomChance < 6)          // 60%
            targetTier = 1;
        else if (randomChance < 9)     // 30%
            targetTier = 2;
        else                           // 10%
            targetTier = 3;
    
        var candidates = _allItems.FindAll(i => i.Tier == targetTier);

        ItemData randomData = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        Item randomItem = CreateItem(randomData, orignalSlot.transform, ItemState.Active);
        StartCoroutine(MoveToSlot(randomItem, emptySlot.transform));
    }
    
    public List<ItemData> GetItemsByTierRange(int minTier, int maxTier)
    {
        return _allItems.FindAll(i => i.Tier >= minTier && i.Tier <= maxTier);
    }


    public void Merge(ItemData itemData, Slot slot, Transform parent)
    {
        slotManager.UnlockHiddenItems(slot);
        ItemData data = _allItems.Find(i => i.ItemType == itemData.ItemType && i.Tier == itemData.Tier + 1);
    
        CreateItem(data, parent, ItemState.Active);
        OnItemMerged?.Invoke();  
    }

    public int GetMaxTierNum(ItemType type)
    {
        int maxTier = -1;
        foreach ( var item in _allItems)
        {
           if(item.ItemType == type && item.Tier > maxTier)
               maxTier = item.Tier;
        }
        return maxTier;
    }
    
    public void SwapItems(Transform slotA, Item itemB)
    {
        StartCoroutine(MoveToSlot(itemB, slotA));
    }
    
    private IEnumerator MoveToSlot(Item itemB, Transform targetSlot)
    {
        RectTransform itemRect = itemB.GetComponent<RectTransform>();
        itemB.SetSlot(targetSlot);
        
        Vector2 start = itemRect.anchoredPosition;
        Vector2 end = Vector2.zero;
        float duration = itemMoveSpeed; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            itemRect.anchoredPosition = Vector2.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        itemRect.anchoredPosition = end;
        
    }
    
 }
