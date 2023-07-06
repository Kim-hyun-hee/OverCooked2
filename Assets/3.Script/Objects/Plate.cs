using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class IngredientIcon
{
    public IngredientIcon(Ingredient ingredient, Image icon)
    {
        this.ingredient = ingredient;
        this.icon = icon;
    }

    public Ingredient ingredient;
    public Image icon;
}

public class Plate : Object
{
    private List<IngredientIcon> ingreicons = new List<IngredientIcon>();
    private Recipe recipe;
    private RecipeManager recipeManager;
    public bool isDirty = false;

    public List<Image> icons = new List<Image>();
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

    private void Update()
    {
        UpdateIconImg();
    }

    private void UpdateIconImg()
    {
        if(icons.Count == 0)
        {
            return;
        }
        if (icons.Count == 1)
        {
            icons[0].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + 65f, Camera.main.WorldToScreenPoint(transform.position).z);
        }
        else if (icons.Count == 2)
        {
            icons[0].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x - 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 65f, Camera.main.WorldToScreenPoint(transform.position).z);
            icons[1].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 65f, Camera.main.WorldToScreenPoint(transform.position).z);
        }
        else if (icons.Count == 3)
        {
            icons[0].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x - 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 72.5f, Camera.main.WorldToScreenPoint(transform.position).z);
            icons[1].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 72.5f, Camera.main.WorldToScreenPoint(transform.position).z);
            icons[2].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x - 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 27.5f, Camera.main.WorldToScreenPoint(transform.position).z);
        }                                                                                                    
        else if (icons.Count == 4)                                                                           
        {                                                                                                    
            icons[0].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x - 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 72.5f, Camera.main.WorldToScreenPoint(transform.position).z);
            icons[1].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 72.5f, Camera.main.WorldToScreenPoint(transform.position).z);
            icons[2].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x - 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 27.5f, Camera.main.WorldToScreenPoint(transform.position).z);
            icons[3].transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x + 22.5f, Camera.main.WorldToScreenPoint(transform.position).y + 27.5f, Camera.main.WorldToScreenPoint(transform.position).z);
        }
    }

    public bool IsDirty()
    {
        return isDirty;
    }

    public List<IngredientIcon> GetIngredients()
    {
        return ingreicons;
    }


    public bool AddIngredients(List<IngredientIcon> ingreicons) // ���ÿ��� ���÷�
    {
        if (isDirty == true)
        {
            return false;
        }

        if (ingreicons.Count == 0)
        {
            return false;
        }

        if (this.ingreicons.Count == 0)
        {
            return false;
        }

        bool isAlright = true;
        Recipe recipe = null;
        foreach (IngredientIcon ingredient in ingreicons)
        {
            this.ingreicons.Add(ingredient);
            recipe = recipeManager.GetRecipe(this.ingreicons);

            if (recipe == null)
            {
                isAlright = false;
                this.ingreicons.RemoveAt(this.ingreicons.Count - 1);
                return false;
            }
            
            // ingredient.ingredient.transform.SetParent(transform.GetChild(0)); // attach point
            // ingredient.ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        if(isAlright)
        {
            foreach (IngredientIcon ingredient in ingreicons)
            {
                ingredient.ingredient.transform.SetParent(transform.GetChild(0)); // attach point
                ingredient.ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
                this.recipe = recipe;
            }
        }

        GetImageIcon(this.ingreicons);
        SetRecipe(this.recipe);
        return true;
    }

    public bool AddIngredient(Ingredient ingredient) // ��� �ϳ�
    {
        IngredientIcon ingreicon = new IngredientIcon(ingredient, ingredient.icon);

        if (isDirty == true)
        {
            return false;
        }

        if (ingredient == null)
        {
            return false;
        }

        if (this.ingreicons.Count == 0)
        {
            if (ingredient.choppable && ingredient.GetState() == State.CHOPPED ||
                ingredient.cookable && ingredient.GetState() == State.COOKED ||
               (!ingredient.cookable && !ingredient.choppable) && ingredient.GetState() == State.RAW)
            {
                this.ingreicons.Add(ingreicon);
                ingredient.transform.SetParent(transform.GetChild(0)); // attach point
                ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
                recipe = recipeManager.GetRecipe(ingreicons); // ��Ḯ��Ʈ
                ingredient.icons.RemoveAt(0);
                GetImageIcon(ingreicons);
                if (recipe!= null)
                {
                    SetRecipe(recipe);
                }
                return true;
            }
            return false;
        }

        ingreicons.Add(ingreicon);
        recipe = recipeManager.GetRecipe(ingreicons); // OrderManager�� ���� �� UI�� ��� Recipe �����ϴ°Ű�
        // RecipeManager �ϳ� ���� ������ ��� ����� �𵨵� �־��ֱ�
        if (recipe == null)
        {
            //ingredient.transform.SetParent(transform.GetChild(0));
            //ingredient.transform.localPosition = new Vector3(0f, (ingredients.Count * 0.003f), 0f);
            ingreicons.RemoveAt(ingreicons.Count - 1);
            return false;
        }
        else
        {
            GetImageIcon(ingreicons);
            SetRecipe(recipe);
        }

        ingredient.transform.SetParent(transform.GetChild(0)); // attach point
        ingredient.transform.localPosition = new Vector3(0f, 0f, 0f);
        return true;
    }

    private void GetImageIcon(List<IngredientIcon> ingredienticons)
    {
        icons = new List<Image>();
        foreach(IngredientIcon ingredienticon in ingredienticons)
        {
            icons.Add(ingredienticon.icon);
        }
    }

    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        //audioManager.Play("Delivery");
        //boom.Play();
        ingreicons.ForEach(ingredient => ingredient.ingredient.gameObject.SetActive(false)); // ����
        foreach (Transform ingredient in transform.GetChild(0)) // ������ ��
        {
            ingredient.gameObject.SetActive(false);
        }
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
            ingreicons = new List<IngredientIcon>();
            icons.ForEach(img => Destroy(img.gameObject));
            icons = new List<Image>();
        }
        else
        {
            ingreicons.ForEach(ingreicon => Destroy(ingreicon.ingredient.gameObject));
            ingreicons = new List<IngredientIcon>();
            icons.ForEach(img => Destroy(img.gameObject));
            icons = new List<Image>();
        }
    }

    public void MoveRecipe()
    {
        foreach (Transform ingredient in transform.GetChild(0))
        {
            Destroy(ingredient.gameObject);
        }
        recipe = null;
        ingreicons = new List<IngredientIcon>();
        icons = new List<Image>();
    }
}

