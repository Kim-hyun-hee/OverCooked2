using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RecipeIngredient
{
    public bool Equals(Ingredient ingredient)
    {
        return ingredient.GetIngredientName() == this.ingredient && ingredient.GetState() == state;
    }

    public IngredientName ingredient;
    public State state;
}

public class Recipe : MonoBehaviour
{
    public List<RecipeIngredient> ingredients;
    public GameObject recipeModel;

    public bool IsRecipe(List<Ingredient> plate)
    {
        //List<RecipeIngredient> notFoundIngredients = new List<RecipeIngredient>(ingredients);
        //foreach (Ingredient plateIngredient in plate)
        //{
        //    bool found = false;
        //    foreach (RecipeIngredient recipeIngredient in notFoundIngredients.ToArray())
        //    {
        //        if (recipeIngredient.Equals(plateIngredient))
        //        {
        //            notFoundIngredients.Remove(recipeIngredient);
        //            found = true;
        //            break;
        //        }
        //    }
        //    if (!found)
        //        return false;
        //}
        //return notFoundIngredients.Count == 0;
        return true;
    }

    public GameObject GetModel()
    {
        return recipeModel;
    }
}
