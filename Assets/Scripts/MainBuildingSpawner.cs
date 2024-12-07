using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuildingSpawner : MonoBehaviour
{
    [SerializeField]
    private float SpawningPeriod;
    private float Timer = 0;
    [SerializeField]
    private List<GameObject> BuildingPrefabs;
    private GameObject SelectedObj;
    private void Update()
    {
        if (Timer < SpawningPeriod/GameManager.Instance.SpeedMultiplier)
        {
            Timer += Time.deltaTime;
        }
        else
        {
            SelectedObj = BuildingPrefabs[Random.Range(0, BuildingPrefabs.Count)];
            Timer = 0;
            Instantiate(SelectedObj, new Vector3(transform.position.x, SelectedObj.transform.position.y, 0), Quaternion.identity);
        }
    }
}
