using System.Collections.Generic;
using UnityEngine.Serialization;

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
    public int energy = 100;
    public List<int> unlockedFurnitureIds = new List<int>();
    public List<int> unlockedItemIds = new List<int>();
}

