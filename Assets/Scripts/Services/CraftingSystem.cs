using System.Collections.Generic;

public class CraftingSystem
{
    private Inventory inventory;
    private List<Recipe> recipes;

    public CraftingSystem(Inventory inventory)
    {
        this.inventory = inventory;
        this.recipes = new List<Recipe>();
    }

    public void AddRecipe(Recipe recipe)
    {
        recipes.Add(recipe);
    }

    public bool CraftItem(Recipe recipe)
    {

        foreach (var ingredient in recipe.Ingredients)
        {
            if (inventory.GetItemQuantity(ingredient.Key) < ingredient.Value)
            {
                return false;
            }
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            inventory.RemoveItem(ingredient.Key, ingredient.Value);
        }

        // Add crafted item to inventory
        inventory.AddItem(recipe.Result.ItemID, 1);
        return true;
    }

    public List<Recipe> GetAllRecipes()
    {
        return new List<Recipe>(recipes);
    }
}