using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;

    private CraftingUIController _craftingUIController;
    private Recipe _recipe;

    public void Initialize(CraftingUIController craftingUIController, Recipe recipe)
    {
        _craftingUIController = craftingUIController;
        _recipe = recipe;
        _buttonText.text = recipe.RecipeName;
        _button.onClick.AddListener(HandleRecipeButtonClick);
    }

    private void HandleRecipeButtonClick()
    {
        _craftingUIController.UpdateRecipeInfo(_recipe);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(HandleRecipeButtonClick);
    }
}
