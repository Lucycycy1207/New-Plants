using System;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private float _coins = 0;

    public Action<float> OnCoinsChanged;

    public static Shop _instance { get; private set; }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }
    public void BuySeed(string name, float price)
    {
        if (_coins >= price)
        {
            _coins -= price;
            Planter._instance.AddSeeds(name, 1);
            OnCoinsChanged(_coins);
        }
        else
        {
            Debug.Log("Not enough coins to buy seeds");
            UIManager._instance.UpdateStatus("Not enough coins");
        }
        
    }

    //Assignment 2
    // Get the harvest, add coins for the value, update UI and remove the item from the data structure
    public void SellHarvest(CollectedHarvest _harvestElement, string _plantName, float pricePerItem, int HarvestAmount)
    {
        //Debug.Log("sellharvest called");
        _coins += pricePerItem * HarvestAmount;
        OnCoinsChanged(_coins);
        Harvester._instance.RemoveHarvest(_harvestElement);
    }

}
