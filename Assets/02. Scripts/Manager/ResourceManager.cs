using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<int, ItemObject> resource = new Dictionary<int, ItemObject>();
    private Dictionary<int, ItemObject> cookRecipe = new Dictionary<int, ItemObject>();
    private Dictionary<int, ItemObject> craftRecipe = new Dictionary<int, ItemObject>();

    public Dictionary<int, ItemObject> Resource { get => resource; }
    public Dictionary<int, ItemObject> CookRecipe { get => cookRecipe; }
    public Dictionary<int, ItemObject> CraftRecipe { get => craftRecipe; }

    private void OnValidate()
    {
        resource.Clear();
        cookRecipe.Clear();
        craftRecipe.Clear();

        ItemData[] resourceData = Resources.LoadAll<ItemData>($"Resource");
        ItemData[] craftRecipeData = Resources.LoadAll<ItemData>($"CraftingRecipe");
        ItemData[] cookRecipeData = Resources.LoadAll<ItemData>($"CookRecipe");

        resource = CreateDictionary(resourceData);
        cookRecipe = CreateDictionary(craftRecipeData);
        craftRecipe = CreateDictionary(cookRecipeData);
    }

    private Dictionary<int, ItemObject> CreateDictionary(ItemData[] itemData)
    {
        Dictionary<int, ItemObject> tempDictionary = new Dictionary<int, ItemObject>();
        foreach (var item in itemData)
        {
            if (!tempDictionary.ContainsKey(item.ItemID))
            {
                tempDictionary.Add(item.ItemID, new ItemObject { data = item });
            }
            else
            {
                Debug.LogWarning($"중복된 ID: {item.ItemID} - {item.name}");
            }
        }
        return tempDictionary;
    }
}
