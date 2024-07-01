using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory
{
    private List<GameObject> _buildingsPrefabs = new List<GameObject>();

    public BuildingFactory(BuildingsSO buildingsSO) 
    {
        for (int i = 0; i < buildingsSO.BuildingsList.Count; i++)
        {
            _buildingsPrefabs.Add(buildingsSO.BuildingsList[i].BuildingPrefab);
        }
    }

    public GameObject Get(int index, Vector3 position)
    {
        return Object.Instantiate(_buildingsPrefabs[index], position, Quaternion.identity);
    }
}
