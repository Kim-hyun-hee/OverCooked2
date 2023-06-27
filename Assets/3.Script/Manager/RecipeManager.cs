using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public List<Recipe> recipes = new List<Recipe>();

    private static RecipeManager instance;
    public static RecipeManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public Recipe GetRecipe(List<Ingredient> plate)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.IsRecipe(plate))
                return recipe;
        }
        return null;
    }
}
