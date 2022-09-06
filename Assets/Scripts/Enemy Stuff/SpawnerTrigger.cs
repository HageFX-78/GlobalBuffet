using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrigger : MonoBehaviour
{
    private bool initialSpawn = true;
    private void OnEnable()
    {
        gameObject.name = $"{transform.localScale.x}";
    }
    private void OnDisable()
    {
        if (initialSpawn)
        {
            initialSpawn = false;
        }
        else
        {
            FoodSpawner.spawner.SpawnFood(1);
        }
    }
}
