using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    [SerializeField] private string _recipeName;
    [SerializeField] private Item _result;
    [SerializeField] private List<Ingredient> _ingredientsList;
    private Dictionary<string, int> _ingredients = new Dictionary<string, int>();

    public string RecipeName
    { get { return _recipeName; } }
    
    public Item Result
    { get { return _result; } }

    public Dictionary<string, int> Ingredients
    { get { return _ingredients; } }

    private void OnEnable()
    {
        HashSet<string> seenItems = new HashSet<string>();
        for (int i = 0; i < _ingredientsList.Count; i++)
        {
            Ingredient ingredient = _ingredientsList[i];
            if (ingredient.ItemID != null && seenItems.Contains(ingredient.ItemID))
            {
                Debug.LogWarning($"Duplicate item '{ingredient.ItemID}' found in recipe '{_recipeName}' at index {i}. Removing duplicate.");
                _ingredientsList.RemoveAt(i);
                i--;
            }
            else if (ingredient.ItemID != null)
            {
                seenItems.Add(ingredient.ItemID);
                _ingredients.Add(ingredient.ItemID, ingredient.Quantity);
            }
        }
    }
}

[Serializable]
public class Ingredient
{
    public string ItemID;
    public int Quantity;
}