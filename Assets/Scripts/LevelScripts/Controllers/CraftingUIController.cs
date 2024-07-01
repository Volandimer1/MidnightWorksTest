using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIController : MonoBehaviour
{
    [SerializeField] Button _craftButton;
    [SerializeField] Image _recipeImage;
    [SerializeField] private Transform _ingredientsContent;
    [SerializeField] private Transform _recipesContent;
    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private GameObject _recipeButtonPrefab;

    private CraftingSystem _craftingSystem;
    private ItemsColectionSO _levelItems;
    private RecipesColectionsSO _levelRecipes;
    private List<InventorySlotView> _slotViews;
    private Recipe _currentRecipe;

    public void Initialize(ItemsColectionSO levelItems, CraftingSystem craftingSystem, RecipesColectionsSO recipesColectionsSO)
    {
        _slotViews = new List<InventorySlotView>();
        _craftingSystem = craftingSystem;
        _levelItems = levelItems;
        _levelRecipes = recipesColectionsSO;
        _craftButton.onClick.AddListener(Craft);

        for (int i = 0; i < _levelRecipes.Recipes.Count; i++)
        {
            GameObject buttonObject = Instantiate(_recipeButtonPrefab, _recipesContent);
            RecipeButton button = buttonObject.GetComponent<RecipeButton>();
            button.Initialize(this, _levelRecipes.Recipes[i]);
        }
    }

    public void UpdateRecipeInfo(Recipe recipe)
    {
        _currentRecipe = recipe;
        _recipeImage.sprite = recipe.Result.Icon;
        foreach (InventorySlotView slot in _slotViews)
        {
            Destroy(slot.gameObject);
        }
        _slotViews.Clear();
        foreach (KeyValuePair<string, int> ingredient in recipe.Ingredients)
        {
            GameObject slotGO = Instantiate(_inventorySlotPrefab, _ingredientsContent);
            InventorySlotView slotView = slotGO.GetComponent<InventorySlotView>();
            Item itemData = _levelItems.ItemsDictionary[ingredient.Key];
            slotView.Initialize(itemData.Icon, ingredient.Value);
            _slotViews.Add(slotView);
        }
    }

    private void Craft()
    {
        _craftingSystem.CraftItem(_currentRecipe);
    }
}
