using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum IngredientName
{
    RICE,
    SEAWEED,
    PRAWN,
    CUCUMBER
};

public enum State
{
    RAW, CHOPPED, COOKED, OVERCOOKED, PLATED
};

public class Ingredient : Object
{
    public IngredientName ingredientName;
    public State state = State.RAW;
    public KitchenTools kitchenTool;

    private AudioSource audioSource;
    
    public bool choppable, cookable, cookCheat = false;
    public float chopTime, cookTime;
    private float overcookTime, remainingOvercookTime, remainingChopTime, remainingCookTime;
    private bool overcooking = false;

    public GameObject raw, chopped, cooked, overcooked, plated;
    private GameObject[] states = new GameObject[5];

    [Header("UI")]
    public Slider slider;
    public Image icon;
    //private Image warning;
    public Transform uiTransform;

    public void Start()
    { 
        //audioSource = GetComponent<AudioSource>();
        states[(int)State.RAW] = raw;
        states[(int)State.CHOPPED] = chopped;
        states[(int)State.COOKED] = cooked;
        states[(int)State.OVERCOOKED] = overcooked;
        states[(int)State.PLATED] = plated;

        //slider = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        //slider.gameObject.SetActive(false);

        //warning = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        uiTransform = GameObject.FindGameObjectWithTag("ObjectUI").transform;

        icon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        icon.transform.SetParent(uiTransform.GetChild(1));
        //warning.gameObject.SetActive(false);

        remainingChopTime = chopTime;
        remainingCookTime = cookTime;
        remainingOvercookTime = overcookTime = cookTime * 0.6f;
    }

    public void Update()
    {
        //if (!overcooking && warning.gameObject.activeSelf)
        //{
        //    warning.gameObject.SetActive(false);
        //}
        //overcooking = false;

        //if (state != State.OVERCOOKED && remainingOvercookTime < (0.75f * overcookTime))
        //{
        //    if (!audioSource.isPlaying)
        //    {
        //        audioSource.Play();
        //    }
        //}
        //else if (audioSource.isPlaying)
        //{
        //    audioSource.Stop();
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    //audioManager.Play("Delivery");
        //    audioSource.Stop();
        //    cookCheat = !cookCheat;
        //}

        icon.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + 70f, Camera.main.WorldToScreenPoint(transform.position).z);
    }

    public IngredientName GetIngredientName()
    {
        return ingredientName;
    }

    public State GetState()
    {
        return state;
    }

    public bool IsChoppable()
    {
        return choppable;
    }

    public bool IsCookable()
    {
        return cookable;
    }

    public void SetState(State state)
    {
        if (this.state != state)
        {
            this.state = state;
            UpdateModel(state);
        }
    }

    public void StartCut()
    {
        StartCoroutine(Cut_co());
    }

    private IEnumerator Cut_co()
    {
        while(true)
        {
            if(choppable && state == State.RAW)
            {
                remainingChopTime -= Time.deltaTime;
                //slider.gameObject.SetActive(true);
                icon.gameObject.SetActive(false);
                //slider.value = (chopTime - remainingChopTime) / chopTime;
                if (remainingChopTime <= 0)
                {
                    SetState(State.CHOPPED);
                    //slider.gameObject.SetActive(false);
                    icon.gameObject.SetActive(true);
                    break;
                }
            }
            yield return null;
        }
    }

    public void StopCut()
    {
        StopAllCoroutines();
    }

    public override void Burn()
    {
    //    if (cookable)
    //    {
    //        SetState(State.OVERCOOKED);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    }

    public bool Cook()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //audioManager.Play("Delivery");
            //SetState(State.COOKED);
            //slider.gameObject.SetActive(false);
        }
        else if (state != State.OVERCOOKED)
        {
            if (state == State.COOKED && !cookCheat)
            {
                overcooking = true;
                remainingOvercookTime -= Time.deltaTime;

                if (remainingOvercookTime <= 0)
                {
                    SetState(State.OVERCOOKED);
                    //warning.gameObject.SetActive(false);
                    //slider.gameObject.SetActive(false);
                    return false;
                }
                else if (remainingOvercookTime <= (0.4f * overcookTime))
                {
                    //warning.gameObject.SetActive(true);
                    //warning.color = Color.red;
                }
                else if (remainingOvercookTime <= (0.75f * overcookTime))
                {
                    //warning.gameObject.SetActive(true);
                }
            }
            else
            {
                remainingCookTime -= Time.deltaTime;
                //slider.gameObject.SetActive(true);
                //slider.value = (cookTime - remainingCookTime) / cookTime;

                if (remainingCookTime <= 0)
                {
                    SetState(State.COOKED);
                    //slider.gameObject.SetActive(false);
                }
            }
        }

        return true;
    }

    private void UpdateModel(State state)
    {
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = states[(int)state].transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        //transform.GetChild(0).GetComponent<Renderer>().materials = states[(int)state].transform.GetChild(0).GetComponent<Renderer>().sharedMaterials;
    }

    override public void ThrowToBin()
    {
        Destroy(gameObject);
    }

}

