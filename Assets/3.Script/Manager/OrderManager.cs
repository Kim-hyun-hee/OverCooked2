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
    public float orderTime = 180.0f;
    public const int TIP = 8;
    public int tip;

    public GameObject uiMoney;
    public GameObject getPrefab, tipPrefab;
    public List<GameObject> uiOrderPrefabs = new List<GameObject>();
    public Transform uiOrders;

    private List<Order> queue = new List<Order>();
    private float timeToNewOrder = 10f;
    private int money;

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
        money = 0;

        if(StageManager.Instance.stageName == StageName.S1_1)
        {
            //uiMoney.transform.GetChild(2).localPosition = new Vector2(1500, 734);
            uiMoney.transform.GetChild(2).localPosition = new Vector2(162.7f, 220.6f);
            uiMoney.transform.GetChild(3).localPosition = new Vector2(1568, 879);
        }
        else if(StageManager.Instance.stageName == StageName.S1_2)
        {
            uiMoney.transform.GetChild(2).localPosition = new Vector2(162.7f, 220.6f);
            uiMoney.transform.GetChild(3).localPosition = new Vector2(1207, 847);
        }
        else
        {
            uiMoney.transform.GetChild(2).localPosition = new Vector2(162.7f, 220.6f);
            uiMoney.transform.GetChild(3).localPosition = new Vector2(385.3f, 511.2f);
        }
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

    public IEnumerator UpdateNewOrder()
    {
        while (true)
        {
            timeToNewOrder -= Time.deltaTime;
            if (timeToNewOrder <= 0.0f && queue.Count < 5)
            {
                timeToNewOrder = 10 + timeToNewOrder;
                int index = Random.Range(0, recipes.Count);
                Order NewOrder = new Order(recipes[index], orderTime);
                queue.Add(NewOrder);
                GameObject UIOrder = InstantiateOrderInUI(NewOrder, index, 0);
                OrderMovement(UIOrder);
            }
            else if (queue.Count == 0)
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
            else if (queue.Count == 1)
            {
                int index = Random.Range(0, recipes.Count);
                Order NewOrder = new Order(recipes[index], orderTime);
                queue.Add(NewOrder);
                GameObject UIOrder = InstantiateOrderInUI(NewOrder, index, 0);
                OrderMovement(UIOrder);
            }
            yield return null;
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

    public IEnumerator UpdateOrders()
    {
        while(true)
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
                    Debug.Log("실패한 주문 / 시간 다 되어서 레시피 사라짐");
                    queue.Remove(order);
                }
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if(this == instance)
        {
            instance = null;
        }
    }

    public Recipe GetRecipe(List<IngredientIcon> plate)
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

    private void SetMoney(int money, int recipe, int tip)
    {
        if(money < 0)
        {
            this.money = 0;
        }
        else
        { 
            this.money = money;
        }
        int get = recipe + tip;
        uiMoney.transform.GetChild(1).GetComponent<Text>().text = this.money.ToString();
        StageManager.Instance.totalScore = this.money;
        InstantiateMoneyUI(getPrefab, "+" + get.ToString(), 2);
        InstantiateMoneyUI(tipPrefab, "+" + tip.ToString() + " Tip!", 3);
        uiMoney.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Animator>().SetTrigger("spin");
    }

    private void InstantiateMoneyUI(GameObject uiPrefab, string text, int i)
    {
        GameObject ui = Instantiate(uiPrefab, uiMoney.transform.GetChild(i));
        ui.transform.localPosition = Vector3.zero;
        ui.GetComponent<Text>().text = text;
        ui.transform.DOLocalMoveY(40, 1);
        ui.GetComponent<Text>().DOFade(0.0f, 1).SetEase(Ease.InQuad);
        StartCoroutine(DestroyMoneyUI_co(ui));
    }

    private IEnumerator DestroyMoneyUI_co(GameObject ui)
    {
        yield return ui.transform.DOLocalMoveY(40, 1).WaitForCompletion();
        Destroy(ui);
    }

    public void AddRecipe(Recipe recipe)
    {
        foreach(Order order in queue)
        {
            if(order.recipe == recipe)
            {
                if(queue.IndexOf(order) == 0)
                {
                    UpdateCombo(++combo);
                }
                else
                {
                    UpdateCombo(0);
                }
                DeleteOrderFromUI(queue.IndexOf(order));
                queue.Remove(order);
                StageManager.Instance.tip += tip;
                StageManager.Instance.successScore += recipe.GetPrice();
                SetMoney(money + recipe.GetPrice() + tip, recipe.GetPrice(), tip);
                SoundManager.Instance.PlaySE("SuccessfulDelivery");
                StageManager.Instance.successOrder += 1;
                return;
            }
        }
        Debug.Log("레시피 모델은 있지만 오더에 없는 음식 / 틀림");
        StageManager.Instance.failOrder += 1;
        Debug.Log("틀린 소리 여기");
        combo = 0;
        uiMoney.transform.GetChild(0).GetChild(2).gameObject.SetActive(false); // flame
        uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(false);
        for (int i = 1; i < 4; i++)
        {
            uiMoney.transform.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(false); // bar
        }
    }

    public void UpdateCombo(int combo)
    {
        if(combo == 0)
        {
            tip = TIP;
            for (int i = 1; i < 4; i++)
            {
                uiMoney.transform.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(false); // bar
            }
            uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(false); // Tip x 0
            uiMoney.transform.GetChild(0).GetChild(2).gameObject.SetActive(false); // flame
            this.combo = combo;
        }
        else
        {
            if(combo > 4)
            {
                combo = 4;
            }
            this.combo = combo;
            tip = combo * TIP;
            if (combo > 1 && combo <= 4)
            {
                uiMoney.transform.GetChild(0).GetChild(1).GetChild(combo - 1).gameObject.SetActive(true); // bar
            }
            uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).gameObject.SetActive(true); // Tip x 0
            uiMoney.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Text>().text = "Tip x " + combo.ToString();
            if (combo == 4)
            {
                uiMoney.transform.GetChild(0).GetChild(2).gameObject.SetActive(true); // flame
            }
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
        //UpdateCombo(0);
    }
}
