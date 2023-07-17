using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KitchenTools
{
    NONE, POT, PAN
};

public class KitchenTool : Object
{
    public KitchenTools tool;
    private Ingredient ingredient;
    private ParticleSystem smoke;
    private ParticleSystem burntSmoke;
    private Image missingIcon;
    public Transform uiTransform;

    public void Start()
    {
        burntSmoke = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        smoke = transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
        uiTransform = GameObject.FindGameObjectWithTag("ObjectUI").transform;
        missingIcon = transform.GetChild(2).GetComponent<Image>();
        missingIcon.transform.SetParent(uiTransform);
        missingIcon.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (HasIngredient() && (GetIngredient().GetState()) != State.OVERCOOKED && (GetIngredient().GetState()) == State.COOKED)
        {
            if (!smoke.isPlaying)
            {
                smoke.Play();
            }
        }
        else
        {
            if (smoke.isPlaying)
            {
                smoke.Stop();
            }
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!HasIngredient())
        {
            missingIcon.gameObject.SetActive(true);
            missingIcon.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.GetChild(0).GetChild(1).position).x, Camera.main.WorldToScreenPoint(transform.GetChild(0).GetChild(1).position).y + 55f, Camera.main.WorldToScreenPoint(transform.GetChild(0).GetChild(1).position).z);
        }
        else
        {
            missingIcon.gameObject.SetActive(false);
        }

    }

    public void PlayBurntSmoke()
    {
        if (!burntSmoke.isPlaying)
        {
            burntSmoke.Play();
        }
    }

    public void StopBurntSmoke()
    {
        if (burntSmoke.isPlaying)
        {
            burntSmoke.Stop();
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
                //transform.GetChild(0).GetChild(1).gameObject.SetActive(true); // 냄비 안에 내용물
                ingredient.transform.SetParent(transform.GetChild(0).GetChild(2));
                ingredient.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                //ingredient.gameObject.SetActive(false); // 임시 조치
            }
            if(tool == KitchenTools.PAN)
            {

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
            //transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            ingredient.icons.ForEach(icon => Destroy(icon));
            SoundManager.Instance.PlaySE("PutDown");
            Destroy(ingredient.gameObject);
            ingredient = null;
        }
        StopBurntSmoke();
    }
}
