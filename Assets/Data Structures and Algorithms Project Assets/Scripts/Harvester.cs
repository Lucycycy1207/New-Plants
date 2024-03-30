using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [SerializeField] public Harvest _harvest;
    [SerializeField] public Seed _seed;

    // Harvest Analytics
    private Dictionary<string, int> _harvests = new Dictionary<string, int>();

    private Dictionary<int, int> _harvestIndex = new Dictionary<int, int>();

    // Harvest to sell
    // Assignment 2 - Data structure to hold collected harvests
    private List<CollectedHarvest> _collectedHarvest = new List<CollectedHarvest>();

    public static Harvester _instance;
       
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }


    public List<CollectedHarvest> GetCollectedHarvest()
    {
        return _collectedHarvest;
    }

    public void RemoveHarvest(CollectedHarvest _harvestElement)
    {
        _collectedHarvest.Remove(_harvestElement);
    }


    //Assignment 2
    /// <summary>
    /// collect the harvest when picked up.
    /// </summary>
    /// <param name="plantName"></param>
    /// <param name="_harvestAmount"></param>
    /// <param name="sprite"></param>
    public void CollectHarvest(string plantName, int _harvestAmount, Sprite sprite)//from Harvest class
    {
        if (_harvests.ContainsKey(plantName))
        {
            _harvests[plantName] += _harvestAmount;
        }
        else
        {
            _harvests.Add(plantName, _harvestAmount);
        }
        CollectedHarvest newHarvest = new CollectedHarvest(plantName, DateTime.Now.ToString(), _harvestAmount);
        _collectedHarvest.Add(newHarvest);

        UIManager._instance.UpdateStatus($"collected {_harvestAmount} of {plantName}");

        UIManager._instance.ShowTotalHarvest(newHarvest);
    }

    public void ShowHarvest(string plantName, int harvestAmount, int seedAmount, Vector2 position)
    {
        // initiate a harvest with random amount
        Harvest harvest = Instantiate(_harvest, position + Vector2.up + Vector2.right, Quaternion.identity);
        harvest.SetHarvest(plantName, harvestAmount);
        
        // initiate one seed object
        Seed seed = Instantiate(_seed, position + Vector2.up + Vector2.left, Quaternion.identity);
        seed.SetSeed(plantName, seedAmount);
    }

    //Assignment 3
    public void SortHarvestByAmount()
    {
        for (int i = 0; i < _collectedHarvest.Count; i++)
        {
            _harvestIndex[i] = i;
        }
        // Sort the collected harvest using Quick sort
        int pivot = _collectedHarvest.Count - 1;
        int index_i = 0;
        int index_j = pivot;

        
        QuickSort(pivot, index_i, index_j);

        UIManager._instance.ClearHarvest();
;
        for (int i = 0; i < _collectedHarvest.Count; i++)
        {
            //Debug.Log(_collectedHarvest[i]._amount);
            UIManager._instance.ShowTotalHarvest(_collectedHarvest[i]);
        }

    }

    private void QuickSort(int pivot_index, int low_index, int high_index)
    {
        
        //Debug.Log($"pivot_index{pivot_index}, low_index{low_index}, high_index{high_index}");
        if (pivot_index <= 0 || high_index - low_index < 0) return;

        int new_low_index = low_index;

        for (int j = low_index; j <= high_index; j++)
        {
            if (_collectedHarvest[j]._amount <= _collectedHarvest[pivot_index]._amount)
            {
                Swap(j, new_low_index);
                //Debug.Log($"swap {j} and {new_low_index}");
                if (new_low_index < pivot_index && j != pivot_index)
                {
                    new_low_index++;
                }
                
            }
        }

        //Sort left side
        QuickSort(new_low_index-1, low_index, new_low_index - 1);

        //Sort right side
        QuickSort(pivot_index, new_low_index+1, pivot_index);

    }

    private void Swap(int index1, int index2)
    {
        CollectedHarvest tempValue;

        tempValue = _collectedHarvest[index1];

        //Update Order
        _collectedHarvest[index1] = _collectedHarvest[index2];
        _collectedHarvest[index2] = tempValue;


    }
}

// For Assignment 2, this holds a collected harvest object
[System.Serializable]
public struct CollectedHarvest
{
    public string _name;
    public string _time;
    public int _amount;
    
    public CollectedHarvest(string name, string time, int amount)
    {
        _name = name;
        _time = time;
        _amount = amount;
    }

}