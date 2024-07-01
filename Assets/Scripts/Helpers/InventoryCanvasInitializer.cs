using UnityEngine;

public class InventoryCanvasInitializer : MonoBehaviour
{
    [SerializeField] private CraftingUIController _craftingUIController;
    [SerializeField] private InventoryUIController _inventoryUIController;
    [SerializeField] private ItemsColectionSO _levelItems;
    [SerializeField] private RecipesColectionsSO _recipesColectionsSO;

    public void Initialize(CraftingSystem craftingSystem, Inventory inventory)
    {
        _craftingUIController.Initialize(_levelItems, craftingSystem, _recipesColectionsSO);
        _inventoryUIController.Initialize(inventory, _levelItems);
    }
}
