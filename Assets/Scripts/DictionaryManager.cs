using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DictionaryManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform contentParent;   // GridLayoutGroup Content
    [SerializeField] private DictionarySlot slotPrefab; // 슬롯 프리팹

    private Dictionary<int, Sprite> _iconCache = new();
    private List<ItemData> _allItems = new();

    private async void OnEnable()
    {
        await LoadAllItems(); // TSV/DB 불러오기
        CreateDictionarySlots();
    }

    private async System.Threading.Tasks.Task LoadAllItems()
    {
        if (_allItems.Count > 0) return; // 이미 불러왔으면 스킵

        _allItems = await TsvLoader.LoadTableAsync<ItemData>("Items");
    }

    private async void CreateDictionarySlots()
    {
        // 기존 슬롯 삭제
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // 플레이어 도감 데이터
        List<int> unlocked = GameManager.Instance.Player.unlockedItemIds;

        foreach (var item in _allItems)
        {
            Sprite icon = await LoadIconAsync(item.Id, item.Icon);

            DictionarySlot slot = Instantiate(slotPrefab, contentParent);
            bool isUnlocked = unlocked.Contains(item.Id);
            slot.SetData(isUnlocked, icon);
        }
    }

    private async System.Threading.Tasks.Task<Sprite> LoadIconAsync(int itemId, string addressKey)
    {
        if (_iconCache.TryGetValue(itemId, out var sprite))
            return sprite;

        var handle = Addressables.LoadAssetAsync<Sprite>(addressKey);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _iconCache[itemId] = handle.Result;
            return handle.Result;
        }
        else
        {
            Debug.LogWarning($"[DictionaryManager] 아이콘 로드 실패: {addressKey}");
            return null;
        }
    }
}