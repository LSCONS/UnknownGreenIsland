using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftingRecipe : MonoBehaviour
{
    private List<ItemObject> cookRecipe = new List<ItemObject>();
    private List<ItemObject> craftRecipe = new List<ItemObject>();

    public List<ItemObject> CookRecipe { get => craftRecipe; }
    public List<ItemObject> CraftRecipe {  get =>  cookRecipe; }

    
    public void AddCookRecipe(ItemObject itemObject)
    {
        if(cookRecipe.Count > 0)
        {
            for(int i = 0; i < cookRecipe.Count; i++)
            {
                if(cookRecipe[i] == itemObject)
                {
                    return;
                }
            }
        }
        cookRecipe.Add(itemObject);
    }


    public void AddCraftRecipe(ItemObject itemObject)
    {
        if (CraftRecipe.Count > 0)
        {
            for (int i = 0; i < CraftRecipe.Count; i++)
            {
                if (CraftRecipe[i] == itemObject)
                {
                    return;
                }
            }
        }
        craftRecipe.Add(itemObject);
    }
}
