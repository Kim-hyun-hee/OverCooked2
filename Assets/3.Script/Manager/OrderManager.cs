using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order
{
    public Order(Recipe recipe, float remainingTime)
    {
        this.recipe = recipe;
        this.remainingTime = remainingTime;
    }

    public Recipe recipe;
    public float remainingTime;
    public Slider slider;
    public GameObject uiOrderPrefab;
} 

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    public static OrderManager Instance { get { return instance; } }

    public List<Recipe> recipes = new List<Recipe>();
    public float levelTime = 180.0f;
    public float remainingTime;
    public float orderTime = 60.0f;
    public int tip = 8;

    public Text uiMoney, uiCrono;
    public Slider cronoSlider;
    private float hue;
    public GameObject EndMenu;
    public GameObject PauseMenu;
    public Transform uiOrders;

    private List<Order> queue = new List<Order>();
    private float timeToNewOrder = 10.0f;
    private int money;
    private bool cronoRunning = true;
    private bool paused = false;

    [System.Serializable]
    public struct OrderImage
    {
        public string name;
        public Sprite menuImg;
        public Sprite ingredientsImg;
    }

    public OrderImage[] images;


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

    public void Start()
    {
        remainingTime = levelTime;
        hue = (float)120 / 360;
        SetMoney(0);
    }

    private void Update()
    {
        UpdateCrono();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
                paused = false;
            }
            else
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                paused = false;
            }
        }
    }

    private void UpdateCrono()
    {
        if (remainingTime > 0 && cronoRunning)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (cronoRunning)
        {
            cronoRunning = false;
            remainingTime = 0;
            Time.timeScale = 0;
            EndMenu.SetActive(true);
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        string minutesString;
        string secondsString;

        if (minutes < 10)
        {
            minutesString = string.Format("0{0}", minutes);
        }
        else
        {
            minutesString = string.Format("{0}", minutes);
        }

        if (seconds < 10)
        {
            secondsString = string.Format("0{0}", seconds);
        }
        else
        {
            secondsString = string.Format("{0}", seconds);
        }

        uiCrono.text = minutesString + ":" + secondsString;
        cronoSlider.value = remainingTime / levelTime;
        float hue = this.hue;
        hue *= cronoSlider.value;
        cronoSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(hue, 1, 0.85f);
    }

    private void InstantiateOrderInUI(Order newOrder)
    {
        GameObject UIOrder = Instantiate(newOrder.uiOrderPrefab, uiOrders.transform);
        newOrder.slider = UIOrder.transform.GetChild(0).GetComponent<Slider>();
        for(int i = 0; i < images.Length; i++)
        {
            if(images[i].name == newOrder.recipe.name)
            {
                UIOrder.GetComponent<Image>().sprite = images[i].menuImg;
                // images[i].ingredientsImg
                break;
            }
        }
    }

    private void UpdateNewOrder()
    {
        timeToNewOrder -= Time.deltaTime;
        if(timeToNewOrder <= 0.0f && queue.Count < 5)
        {
            timeToNewOrder = 15;
            Order NewOrder = new Order(recipes[Random.Range(0, recipes.Count)], orderTime);
            queue.Add(NewOrder);
            InstantiateOrderInUI(NewOrder);
        }
    }

    private void UpdateOrders()
    {
        foreach (Order order in queue.ToArray())
        {
            order.remainingTime -= Time.deltaTime;
            order.slider.value = 1 - ((orderTime - order.remainingTime) / orderTime);
            if (order.remainingTime <= 0)
            {
                DeleteOrderFromUI(queue.IndexOf(order));
                queue.Remove(order);
                SetMoney(money - 15);
            }
        }
    }

    public void Resume()
    {

        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        paused = false;

    }

    private void OnDestroy()
    {
        if(this == instance)
        {
            instance = null;
        }
    }

    public Recipe GetRecipe(List<Ingredient> plate)
    {
        foreach(Recipe recipe in recipes)
        {
            if(recipe.IsRecipe(plate))
            {
                return recipe;
            }
        }
        return null;
    }

    private void SetMoney(int money)
    {
        if(money < 0)
        {
            this.money = 0;
        }
        else
        { 
            this.money = money;
        }
        
        uiMoney.text = this.money.ToString();
    }

    public void AddCompletedRecipe(Recipe recipe)
    {
        foreach(Order order in queue)
        {
            if(order.recipe == recipe)
            {
                //DeleteOrderFromUI(queue.IndexOf(order));
                //queue.Remove(order);
                SetMoney(money + recipe.GetPrice()); // 팁 추가 해주기
                return;

            }
        }
    }

    private void DeleteOrderFromUI(int index)
    {
        Destroy(uiOrders.GetChild(index).gameObject);
    }
}
