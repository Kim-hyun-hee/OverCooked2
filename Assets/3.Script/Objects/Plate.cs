using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Object
{
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();
    private Recipe recipe;
    //private ParticleSystem boom;

    //new public void Awake()
    //{
    //    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    //    boom = transform.GetChild(2).GetComponent<ParticleSystem>();
    //}

    public bool AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            return false;
        }

        if (ingredients.Count == 0)
        {
            if(ingredient.choppable && ingredient.GetState() == State.CHOPPED ||
                ingredient.cookable && ingredient.GetState() == State.COOKED || 
               (!ingredient.cookable && !ingredient.choppable) && ingredient.GetState() == State.RAW)
            {
                ingredients.Add(ingredient);
                ingredient.transform.SetParent(transform.GetChild(0)); // attach point
                ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
                return true;
            }
            return false;
        }

        ingredients.Add(ingredient);
        recipe = RecipeManager.Instance.GetRecipe(ingredients); // OrderManager�� ���� �� UI�� ��� Recipe �����ϴ°Ű�
        // RecipeManager �ϳ� ���� ������ ��� ����� �𵨵� �־��ֱ�
        if (recipe == null)
        {
            //ingredient.transform.SetParent(transform.GetChild(0));
            //ingredient.transform.localPosition = new Vector3(0f, (ingredients.Count * 0.003f), 0f);
            ingredients.RemoveAt(ingredients.Count - 1);
            return false;
        }
        else
        { 
            SetRecipe(recipe);
        }

        return true;
    }

    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        //audioManager.Play("Delivery");
        //boom.Play();
        ingredients.ForEach(ingredient => Destroy(ingredient.gameObject));
        //ingredients = new List<Ingredient>();

        GameObject recipeModel = Instantiate(recipe.GetModel());
        recipeModel.transform.SetParent(transform.GetChild(0)); // attach point
        recipeModel.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public override void Burn() { }

    public bool IsRecipe()
    {
        return recipe != null;
    }

    public Recipe GetRecipe()
    {
        return recipe;
    }

    public override void ThrowToBin()
    {
        if (IsRecipe())
        {
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
            recipe = null;
            ingredients = new List<Ingredient>();
        }
        else
        {
            ingredients.ForEach(ingredient => Destroy(ingredient.gameObject));
            ingredients = new List<Ingredient>();
        }
    }
}

