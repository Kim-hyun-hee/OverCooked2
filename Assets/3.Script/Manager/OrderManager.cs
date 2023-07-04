using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Order
{
    public Order(Recipe recipe, float remainingTime)
    {
        this.recipe = recipe;
        this.remainingTime = remainingTime;
    }

    public Recipe recipe;
    public float remainingTime;
    public Slider[] sliders = new Slider[3];
    public float sizeX;
} 

public class OrderManager : MonoBehaviour
{
    private static OrderManager instance;
    public static OrderManager Instance { get { return instance; } }

    public List<Recipe> recipes = new List<Recipe>();
    public float levelTime = 180.0f;
    public float remainingTime;
    public float orderTime = 180.0f;
    public const int TIP = 8;
    public int tip;

    public GameObject uiMoney, uiCrono;
    public Slider cronoSlider;
    private float hue;
    public GameObject EndMenu;
    public GameObject PauseMenu;
    public List<GameObject> uiOrderPrefabs = new List<GameObject>();
    public Transform uiOrders;

    private List<Order> queue = new List<Order>();
    private float timeToNewOrder = 10f;
    private int money;
    private bool cronoRunning = true;
    private bool paused = false;

    private int combo;


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
        UpdateNewOrder();
        UpdateOrders();
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

        int minutes = (Mathf.CeilToInt(remainingTime) / 60);
        int seconds = Mathf.CeilToInt(remainingTime % 60);
        if(seconds == 60)
        {
            seconds = 0;
        }

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

        uiCrono.transform.GetChild(2).GetComponent<Text>().text = minutesString + ":" + secondsString;
        cronoSlider.value = remainingTime / levelTime;
        float hue = this.hue;
        hue *= cronoSlider.value;
        cronoSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.HSVToRGB(hue, 1, 0.85f);
    }

    private GameObject InstantiateOrderInUI(Order newOrder, int index, int add)
    {
        GameObject UIOrder = Instantiate(uiOrderPrefabs[index], uiOrders.transform);
        for(int i = 0; i < 3; i++)
        {
            newOrder.sliders[i] = UIOrder.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(i).GetComponent<Slider>();
        }
        newOrder.sizeX = UIOrder.GetComponent<BoxCollider2D>().size.x;
        UIOrder.transform.localPosition = new Vector3(960 + newOrder.sizeX / 2 + add, UIOrder.transform.localPosition.y, 0);
        return UIOrder;
    }

    private void UpdateNewOrder()
    {
        timeToNewOrder -= Time.deltaTime;
        if(timeToNewOrder <= 0.0f && queue.Count < 5)
        {
            timeToNewOrder = 10 + timeToNewOrder;
            int index = Random.Range(0, recipes.Count);
            Order NewOrder = new Order(recipes[index], orderTime);
            queue.Add(NewOrder);
            GameObject UIOrder = InstantiateOrderInUI(NewOrder, index, 0);
            OrderMovement(UIOrder);
        }
        else if (timeToNewOrder > 0 && queue.Count == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                int index = Random.Range(0, recipes.Count);
                Order NewOrder = new Order(recipes[index], orderTime);
                queue.Add(NewOrder);
                GameObject UIOrder = InstantiateOrderInUI(NewOrder, index, i * 210);
                OrderMovement(UIOrder);
            }
        }
        else if (timeToNewOrder > 0 && queue.Count == 1)
        {
            int index = Random.Range(0, recipes.Count);
            Order NewOrder = new Order(recipes[index], orderTime);
            queue.Add(NewOrder);
            GameObject UIOrder = InstantiateOrderInUI(NewOrder, index, 0);
            OrderMovement(UIOrder);
        }
    }

    private void OrderMovement(GameObject UIOrder) // 좌로 이동
    {
        float targetX = -950;
        for(int i = 0; i < queue.Count -1; i++)
        {
            targetX += queue[i].sizeX;
        }
        targetX += queue[queue.Count - 1].sizeX / 2;
        UIOrder.transform.DOLocalMoveX(targetX, 0.5f);
    }

    private void UpdateOrders()
    {
        foreach (Order order in queue.ToArray())
        {
            order.remainingTime -= Time.deltaTime;
            
            if(order.remainingTime > orderTime * 2 / 3)
            {
                order.sliders[2].value = (((order.remainingTime * 3) / orderTime) - 2);
            }
            else if(order.remainingTime > orderTime / 3)
            {
                order.sliders[2].value = 0;
                order.sliders[1].value = ( ((order.remainingTime * 3) / orderTime) - 1);
            }
            else
            {
                order.sliders[1].value = 0;
                order.sliders[0].value =  ((order.remainingTime * 3) / orderTime);
            }

            if (order.remainingTime <= 0) // 시간 다 되어서 사라짐
            {
                order.sliders[0].value = 0;
                DeleteOrderFromUI(queue.IndexOf(order));
                queue.Remove(order);
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
        
        uiMoney.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = this.money.ToString();
    }

    public void AddRecipe(Recipe recipe)
    {
        foreach(Order order in queue)
        {
            if(order.recipe == recipe)
            {
                if(queue.IndexOf(order) == 0)
                {
                    if(combo != 4)
                    {
                        combo++;
                        tip = combo * TIP;

                        if(combo > 1 && combo <= 4)
                        {
                            uiMoney.transform.GetChild(0).GetChild(1).GetChild(combo - 1).gameObject.SetActive(true); // bar
                        }
                        uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(true); // Tip x 0
                        uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Text>().text = "Tip x " + combo.ToString();
                    }
                    if(combo == 4)
                    {
                        uiMoney.transform.GetChild(0).GetChild(3).gameObject.SetActive(true); // flame
                    }
                }
                else
                {
                    combo = 0;
                    tip = TIP;
                    
                    for(int i = 1; i < 4; i++)
                    {
                        uiMoney.transform.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(false); // bar
                    }
                    uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(false); // Tip x 0
                    uiMoney.transform.GetChild(0).GetChild(3).gameObject.SetActive(false); // flame
                }
                DeleteOrderFromUI(queue.IndexOf(order));
                queue.Remove(order);
                SetMoney(money + recipe.GetPrice() + tip);
                uiMoney.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Animator>().SetTrigger("spin");
                return;
            }
        }
        Debug.Log("레시피 모델은 있지만 오더에 없는 음식");
        combo = 0;
        uiMoney.transform.GetChild(0).GetChild(3).gameObject.SetActive(false); // flame
        uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(false);
        for (int i = 1; i < 4; i++)
        {
            uiMoney.transform.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(false); // bar
        }
    }

    private void DeleteOrderFromUI(int index)
    {
        float moveX = queue[index].sizeX;
        Destroy(uiOrders.GetChild(index).gameObject);
        for(int i = index + 1; i < queue.Count; i++)
        {
            uiOrders.GetChild(i).DOLocalMoveX(uiOrders.GetChild(i).localPosition.x - moveX, 0.2f);
        }
    }
}
