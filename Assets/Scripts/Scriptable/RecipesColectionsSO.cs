using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipesColectionsSO : ScriptableObject
{
    [SerializeField] private List<Recipe> _recipesList;

    public List<Recipe> Recipes
    { get { return _recipesList; } }

    private void OnEnable()
    {
        HashSet<Recipe> seenRecipes = new HashSet<Recipe>();
        for (int i = 0; i < _recipesList.Count; i++)
        {
            Recipe recipe = _recipesList[i];
            if (recipe != null && seenRecipes.Contains(recipe))
            {
                Debug.LogWarning($"Duplicate item '{recipe.name}' found in _recipesList at index {i}. Removing duplicate.");
                _recipesList.RemoveAt(i);
                i--;
            }
            else if (recipe != null)
            {
                seenRecipes.Add(recipe);
            }
        }
    }
}
