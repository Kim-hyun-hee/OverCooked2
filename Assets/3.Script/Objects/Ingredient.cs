using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

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
    
    public bool choppable, cookable = false;
    public float chopTime, cookTime, overcookTime;
    public float remainingOvercookTime, remainingChopTime, remainingCookTime;
    //private bool overcooking = false;

    public GameObject raw, chopped, cooked, overcooked, plated;
    private GameObject[] states = new GameObject[5];

    [Header("UI")]
    public Image icon;
    public Slider slider;
    public Image done;
    //public Image warning;
    public Image burn;
    //private Image warning;
    public Transform uiTransform;
    public List<GameObject> icons = new List<GameObject>();

    public void Start()
    { 
        //audioSource = GetComponent<AudioSource>();
        states[(int)State.RAW] = raw;
        states[(int)State.CHOPPED] = chopped;
        states[(int)State.COOKED] = cooked;
        states[(int)State.OVERCOOKED] = overcooked;
        states[(int)State.PLATED] = plated;

        uiTransform = GameObject.FindGameObjectWithTag("ObjectUI").transform;

        icon = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        slider = transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        done = transform.GetChild(1).GetChild(2).GetComponent<Image>();
       // warning = transform.GetChild(1).GetChild(3).GetComponent<Image>();
        burn = transform.GetChild(1).GetChild(3).GetComponent<Image>();

        icons.Add(icon.gameObject);
        icons.Add(slider.gameObject);
        icons.Add(done.gameObject);
        //icons.Add(warning.gameObject);
        icons.Add(burn.gameObject);

        icon.transform.SetParent(uiTransform);
        slider.transform.SetParent(uiTransform);
        done.transform.SetParent(uiTransform);
        //warning.transform.SetParent(uiTransform);
        burn.transform.SetParent(uiTransform);

        slider.gameObject.SetActive(false);                                                                        
        done.gameObject.SetActive(false);
       // warning.gameObject.SetActive(false);
        burn.gameObject.SetActive(false);

        remainingChopTime = chopTime;
        remainingCookTime = cookTime;
        remainingOvercookTime = overcookTime;
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
        UpdateIconImg();
        if(cookable)
        {
            UpdateUI();
        }
        else if(choppable)
        {
            slider.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + 65f, Camera.main.WorldToScreenPoint(transform.position).z);
        }
    }

    private void UpdateIconImg()
    {
        icon.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + 55f, Camera.main.WorldToScreenPoint(transform.position).z);
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
        while (true)
        {
            if(choppable && state == State.RAW)
            {
                remainingChopTime -= Time.deltaTime;
                slider.gameObject.SetActive(true);
                icon.gameObject.SetActive(false);
                slider.value = (chopTime - remainingChopTime) / chopTime;
                if (remainingChopTime <= 0)
                {
                    SetState(State.CHOPPED);
                    slider.gameObject.SetActive(false);
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

    public void UpdateUI()
    {
        //warning.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y - 65f, Camera.main.WorldToScreenPoint(transform.position).z);
        burn.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + 55f, Camera.main.WorldToScreenPoint(transform.position).z);
        slider.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y - 65f, Camera.main.WorldToScreenPoint(transform.position).z);
    }

    public bool Cook()
    {
        if (state != State.OVERCOOKED)
        {
            if (state == State.COOKED)
            {
                //overcooking = true;
                remainingOvercookTime -= Time.deltaTime;

                if (remainingOvercookTime <= 0)
                {
                    SetState(State.OVERCOOKED);
                    //warning.gameObject.SetActive(false);
                    slider.gameObject.SetActive(false);
                    //burn.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y + 65f, Camera.main.WorldToScreenPoint(transform.position).z);
                    burn.gameObject.SetActive(true);
                    icon.gameObject.SetActive(false);

                    return false;
                }
                else if (remainingOvercookTime <= (0.75f * overcookTime))
                {
                    //warning.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y - 65f, Camera.main.WorldToScreenPoint(transform.position).z);
                    //done.gameObject.SetActive(false);
                    //warning.gameObject.SetActive(true);
                }
            }
            else
            {
                remainingCookTime -= Time.deltaTime;
                //slider.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y - 65f, Camera.main.WorldToScreenPoint(transform.position).z);
                slider.gameObject.SetActive(true);
                slider.value = (cookTime - remainingCookTime) / cookTime;

                if (remainingCookTime <= 0)
                {
                    SetState(State.COOKED);
                    slider.gameObject.SetActive(false);
                    done.transform.position = new Vector3(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y - 65f, Camera.main.WorldToScreenPoint(transform.position).z);
                    done.gameObject.SetActive(true);
                }
            }
        }

        return true;
    }

    private void UpdateModel(State state)
    {
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = states[(int)state].transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        transform.GetChild(0).GetComponent<MeshCollider>().sharedMesh = states[(int)state].transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        //transform.GetChild(0).GetComponent<Renderer>().materials = states[(int)state].transform.GetChild(0).GetComponent<Renderer>().sharedMaterials;
    }

    override public void ThrowToBin()
    {
        icons.ForEach(icon => Destroy(icon));
        Destroy(gameObject);
    }

}

