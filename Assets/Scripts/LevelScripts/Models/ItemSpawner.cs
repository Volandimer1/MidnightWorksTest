using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemFactory _itemFactory;
    [SerializeField] private string itemID1;
    [SerializeField] private string itemID2;
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 5f;

    private float nextSpawnTime;

    public void Initialize(ItemFactory itemFactory)
    {
        _itemFactory = itemFactory;
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnItem();
            ScheduleNextSpawn();
        }
    }

    private void SpawnItem()
    {
        string itemID = Random.Range(0, 2) == 0 ? itemID1 : itemID2;
        _itemFactory.GetItemDrop(itemID, transform.position);
    }

    private void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }
}