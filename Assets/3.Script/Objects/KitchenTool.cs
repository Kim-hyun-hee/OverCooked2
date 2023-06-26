using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KitchenTools
{
    NONE, POT
};

public class KitchenTool : Object
{
    public KitchenTools tool;
    private Ingredient ingredient;
    private ParticleSystem smoke;

    public void Start()
    {
        //smoke = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if(HasIngredient())
        {
            //if(!smoke.isPlaying)
            //{
            //    smoke.Play();
            //}
            //else
            //{
            //    smoke.Stop();
            //}
        }
    }

    public bool Cook()
    {
        if(ingredient != null)
        {
            return ingredient.Cook();
        }
        return true;
    }

    public bool HasIngredient()
    {
        return ingredient != null;
    }

    public override void Burn()
    {
        if(ingredient != null)
        {
            ingredient.Burn();
        }
    }

    public bool AddIngredient(Ingredient ingredient)
    {
        if(this.ingredient == null && IsValid(ingredient))
        {
            if(tool == KitchenTools.POT)
            {
                transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                ingredient.transform.SetParent(transform.GetChild(0).GetChild(1));
                ingredient.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                ingredient.gameObject.SetActive(false); // 임시 조치
            }
            this.ingredient = ingredient;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsValid(Ingredient ingredient)
    {
        return ingredient.cookable && ingredient.kitchenTool == tool
            && ((ingredient.choppable && ingredient.GetState() == State.CHOPPED) || (!ingredient.choppable && ingredient.GetState() == State.RAW));
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    public void DeleteIngredient()
    {
        ingredient = null;
    }

    override public void ThrowToBin()
    {
        if (ingredient != null)
        {
            Destroy(ingredient.gameObject);
            ingredient = null;
        }
    }
}
