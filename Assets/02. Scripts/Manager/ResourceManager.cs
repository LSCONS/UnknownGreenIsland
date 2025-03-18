using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private List<ItemObject> resource = new List<ItemObject>();
    private List<ItemObject> cookRecipe = new List<ItemObject>();
    private List<ItemObject> craftRecipe = new List<ItemObject>();

    public List<ItemObject> Resource { get => resource; }
    public List<ItemObject> CookRecipe { get => craftRecipe; }
    public List<ItemObject> CraftRecipe { get => cookRecipe; }


}
