using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public List<Recipe> recipes = new List<Recipe>();

    public Recipe GetRecipe(List<IngredientIcon> plate)
    {
        foreach (Recipe recipe in recipes)
        {
            if(recipe.IsRecipe(plate))
            {
                return recipe;
            }
        }
        return null;
    }
}
