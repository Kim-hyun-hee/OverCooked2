using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    public static OrderManager Instance { get { return instance; } }

    public List<Recipe> recipes = new List<Recipe>();


    private void Awake()
    {
        if(instance != null && instance != this)
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
