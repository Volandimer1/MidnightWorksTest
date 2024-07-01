using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Canvas _canvas;

    private Inventory _inventory;
    private Item _item;
    private float amplitude = 0.5f;
    private float frequency = 1f;
    private Vector3 startPosition;

    public void Initialize(Item item, Inventory inventory, Camera camera)
    {
        _inventory = inventory;
        _item = item;
        _image.sprite = _item.Icon;
        startPosition = transform.position;
        _canvas.worldCamera = camera;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _inventory.AddItem(_item.ItemID, 1);
        Destroy(gameObject);
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}