using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Plate : Object
{
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField] private Recipe recipe;
    private RecipeManager recipeManager;
    //private ParticleSystem boom;

    //new public void Awake()
    //{
    //    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    //    boom = transform.GetChild(2).GetComponent<ParticleSystem>();
    //}

    private void Start()
    {
        recipeManager = FindObjectOfType<RecipeManager>();
    }

    public bool AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            return false;
        }

        if (ingredients.Count == 0)
        {
            if (ingredient.choppable && ingredient.GetState() == State.CHOPPED ||
                ingredient.cookable && ingredient.GetState() == State.COOKED ||
               (!ingredient.cookable && !ingredient.choppable) && ingredient.GetState() == State.RAW)
            {
                ingredients.Add(ingredient);
                ingredient.transform.SetParent(transform.GetChild(0)); // attach point
                ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
                recipe = recipeManager.GetRecipe(ingredients);
                if(recipe!= null)
                {
                    SetRecipe(recipe);
                }
                return true;
            }
            return false;
        }

        ingredients.Add(ingredient);
        recipe = recipeManager.GetRecipe(ingredients); // OrderManager는 왼쪽 위 UI에 띄울 Recipe 관리하는거고
        // RecipeManager 하나 만들어서 가능한 모든 경우의 모델들 넣어주기
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

        ingredient.transform.SetParent(transform.GetChild(0)); // attach point
        ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
        return true;
    }

    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        //audioManager.Play("Delivery");
        //boom.Play();
        ingredients.ForEach(ingredient => ingredient.gameObject.SetActive(false));
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
            foreach(Transform ingredient in transform.GetChild(0))
            {
                Destroy(ingredient.gameObject);
            }
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

