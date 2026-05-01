using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxCapacity = 10;

    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    public int CurrentCapacity
    {
        get
        {
            int total = 0;
            foreach (int amount in resources.Values)
            {
                total += amount;
            }
            return total;
        }
    }

    public bool AddResource(ResourceType type, int amount)
    {
        if (CurrentCapacity + amount > maxCapacity)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        if (!resources.ContainsKey(type))
        {
            resources[type] = 0;
        }

        resources[type] += amount;
        Debug.Log($"Collected {amount} {type}. Total: {resources[type]}");

        return true;
    }

    public bool HasResource(ResourceType type, int amount)
    {
        return resources.ContainsKey(type) && resources[type] >= amount;
    }

    public bool SpendResource(ResourceType type, int amount)
    {
        if (!HasResource(type, amount)) return false;

        resources[type] -= amount;
        return true;
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resources.ContainsKey(type) ? resources[type] : 0;
    }

    public void IncreaseCapacity(int amount)
    {
        maxCapacity += amount;
    }
}