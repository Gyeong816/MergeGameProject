using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public int Id { get; set; }       
    public string Name { get; set; }  
    public string Icon { get; set; }
    public ItemType ItemType { get; set; }
    public int Tier { get; set; }   
}

[System.Serializable]
public class PlayerData
{
    public int gold = 0;
    public int stamina = 10;
    public List<SlotData> slots = new();
}

[System.Serializable]
public class SlotData
{
    public int slotIndex;
    public string itemId;
    public int itemLevel;
    public bool isEmpty;
}