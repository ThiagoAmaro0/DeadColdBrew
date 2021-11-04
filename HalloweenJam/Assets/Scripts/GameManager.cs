using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //stats
    public int money;
    private Coffee coffee;
    private Food food;
    Customer customer;
    Status status;
    bool goodFood;
    //data
    [SerializeField] Customer[] customers;
    [SerializeField] Sprite[] coffeeIcon, foodIcon;
    [SerializeField] float cookingTolerance, customerTimer;
    [SerializeField] int coffeePrice, foodPrice;
    float timer;
    //UI
    [SerializeField] Image coffeeMural, foodMural, fade;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] Button doneButton;
    [SerializeField] Slider timerSlider;
    [SerializeField] SpriteRenderer coffeeSpr, foodSpr;
    [SerializeField] GameObject ghostText, customerText, madelineText;

    [SerializeField] GameObject[] customerPref;
    [SerializeField] GameObject madelineGO;
    [SerializeField] Animator cardAnim;
    [SerializeField] Transform barPos;
    GameObject actualCustomerGO;
    Vector3 startPos;

    [SerializeField] string[] madelineDialogues;
    [SerializeField] string[] customersDialogues;
    [SerializeField] string[] reverseDialogues;
    [SerializeField] string[] goodReply;
    [SerializeField] string[] badReply;
    [SerializeField] string[] ghostReply;
    int madelineIndex; 
    public CardSystem cardSystem;
    public static GameManager instance;
    int day;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Play("Game");
        day = 1;
        instance = this;
        cardSystem = GetComponent<CardSystem>();
        startPos = madelineGO.transform.position;
        
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        madelineIndex = 0;
        cardSystem.running = false;
        status = Status.MadelaineWalk;
        madelineGO.transform.DOMove(barPos.position, 2);
        doneButton.interactable = false;
        cardSystem.RandomCard();
        timer = customerTimer;
        fade.GetComponentInChildren<TextMeshProUGUI>().text = "Day " + day;
        yield return new WaitForSeconds(3);
        fade.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0,1,false);
        fade.DOFade(0, 2);

    }
    // Update is called once per frame
    void Update()
    {        
        timerSlider.value = timer / customerTimer;
        moneyText.text = "" + money;
        timer -= Time.deltaTime;
        if (timer <= 0 && status != Status.Close)
        {
            fade.DOFade(1,3);
            status = Status.Close;
            if (actualCustomerGO)
                actualCustomerGO.transform.DOMove(startPos,2);
            if (madelineGO)
                madelineGO.transform.DOMove(startPos,2);
            doneButton.interactable = false;
            day++;
            
        }
        switch (status)
        {
            case Status.MadelaineWalk:
                if(madelineGO.transform.position == barPos.position)
                {
                    status = Status.Tarot;
                    StartCoroutine(Move(madelineText.transform, -1.125f, 0));
                    doneButton.interactable = true;
                }
                break;
            case Status.Tarot:
                madelineText.GetComponentInChildren<TextMeshProUGUI>().text = madelineDialogues[madelineIndex];
                break;
            case Status.MadelaineExit:
                if (madelineGO.transform.position == startPos)
                {
                    status = Status.Exit;                    
                }
                break;
            case Status.NewCustomer:
                if (actualCustomerGO.transform.position == barPos.position)
                {
                    status = Status.Hello;
                    StartCoroutine(Move(ghostText.transform, -1.775f, 0));
                    StartCoroutine(Move(customerText.transform, -1.125f, 0));
                    customerText.GetComponentInChildren<TextMeshProUGUI>().text = cardSystem.IsWOF() ? reverseDialogues[Random.Range(0, customersDialogues.Length)] : customersDialogues[Random.Range(0, customersDialogues.Length)];
                    doneButton.interactable = true;
                    ghostText.GetComponentInChildren<TextMeshProUGUI>().text = ghostReply[Random.Range(0, ghostReply.Length)];
                }
                break;
            case Status.Done:
                if (actualCustomerGO.transform.position == startPos)
                {
                    status = Status.Exit;
                }
                break;
            case Status.Exit:
                NewCustomer();
                status = Status.NewCustomer;
                break;
            case Status.Close:
                if (fade.color.a == 1)
                {
                    fade.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(1, 1, false);
                    StartCoroutine(StartGame());
                }
                break;
            default:
                break;
        }
    }

    public void NewCustomer()
    {
       
        if (actualCustomerGO)
            Destroy(actualCustomerGO);
        actualCustomerGO = Instantiate(customerPref[Random.Range(0,customerPref.Length)], barPos.parent);
        startPos = actualCustomerGO.transform.position;
        actualCustomerGO.transform.DOMove(barPos.position,2);
        
        customer = customers[Random.Range(0, customers.Length)];
    }

    public void SetCoffee(Coffee _coffee)
    {
        coffeeSpr.color = new Color(1, 1, 1, 1);
        coffee = _coffee;
        coffeeSpr.sprite = GetCoffee(coffee);
        if(status == Status.Order)
            doneButton.interactable = coffee != null && food != null;

    }

    public void SetFood(Food _food, Food recipe)
    {
        foodSpr.color = new Color(1, 1, 1, 1);
        food = _food;
        foodSpr.sprite = GetFood(_food, recipe);
        int time = food.cookingTime - recipe.cookingTime;
        goodFood = !(-cookingTolerance > time || food.temp < recipe.temp) && !(cookingTolerance < time || food.temp > recipe.temp);
        if (status == Status.Order)
            doneButton.interactable = coffee != null && food != null;
    }
    public Sprite GetFood(Food _food, Food recipe)
    {
        Sprite sprite = null;
        int time = _food.cookingTime - recipe.cookingTime;
        if (-cookingTolerance > time || _food.temp < recipe.temp)
        {
            switch (_food.name)
            {
                case "Cookie":
                    sprite = foodIcon[0];
                    break;
                case "Croissant":
                    sprite = foodIcon[1];
                    break;
                case "Cinnamon Roll":
                    sprite = foodIcon[2];
                    break;
            }
        }
        else if (cookingTolerance < time || _food.temp > recipe.temp)
        {
            switch (_food.name)
            {
                case "Cookie":
                    sprite = foodIcon[3];
                    break;
                case "Croissant":
                    sprite = foodIcon[4];
                    break;
                case "Cinnamon Roll":
                    sprite = foodIcon[5];
                    break;
            }
        }
        else
        {
            switch (_food.name)
            {
                case "Cookie":
                    sprite = foodIcon[6];
                    break;
                case "Croissant":
                    sprite = foodIcon[7];
                    break;
                case "Cinnamon Roll":
                    sprite = foodIcon[8];
                    break;
            }
        }
        return sprite;
    }
    Sprite GetCoffee(Coffee coffee)
    {
        Sprite sprite;
        if (coffee.cold)
            if (coffee.cream)
                if (coffee.choco)
                    sprite = coffeeIcon[0];
                else
                    sprite = coffeeIcon[1];
            else
                if (coffee.choco)
                sprite = coffeeIcon[2];
            else
                sprite = coffeeIcon[3];
        else
            if (coffee.cream)
            if (coffee.choco)
                sprite = coffeeIcon[4];
            else
                sprite = coffeeIcon[5];
        else
                if (coffee.choco)
            sprite = coffeeIcon[6];
        else
            sprite = coffeeIcon[7];
        return sprite;
    }
    Sprite GetReverseCoffee(Coffee coffee)
    {
        Sprite sprite;
        if (!coffee.cold)
            if (!coffee.cream)
                if (!coffee.choco)
                    sprite = coffeeIcon[0];
                else
                    sprite = coffeeIcon[1];
            else
                if (!coffee.choco)
                    sprite = coffeeIcon[2];
                else
                    sprite = coffeeIcon[3];
        else
            if (!coffee.cream)
                if (!coffee.choco)
                    sprite = coffeeIcon[4];
                else
                    sprite = coffeeIcon[5];
        else
                if (!coffee.choco)
                    sprite = coffeeIcon[6];
                else
                    sprite = coffeeIcon[7];
        return sprite;
    }

    public void Done()
    {
        AudioManager.Play("Ding");
        if(status == Status.Tarot)
        {
            madelineIndex++;
            if (madelineIndex == madelineDialogues.Length)
            {
                status = Status.MadelaineExit;
                madelineGO.transform.DOMove(startPos, 2);
                StartCoroutine(Move(madelineText.transform, 2, 1.5f));
            }
            else if (madelineIndex == 1)
            {
                cardAnim.SetBool("PopUp",true);
                doneButton.interactable = false;
            }
            else if (madelineIndex == 2)
            {
                doneButton.interactable = true;
                cardAnim.SetBool("PopUp", false);
            }
        }
        else if (status == Status.Order)
        {
            AudioManager.Play("Money");
            cardSystem.running = false;
            status = Status.Done;
            actualCustomerGO.transform.DOMove(startPos, 2);
            int success = 0;
            if (food.name == customer.food.name && goodFood)
            {
                Debug.Log("BoaComida");
                success++;
                money += foodPrice;
            }
            if (coffee.cream == customer.coffee.cream && coffee.choco == customer.coffee.choco && coffee.cold == customer.coffee.cold)
            {
                Debug.Log("BoaCafe");
                success++;
                money += coffeePrice;
            }
            customerText.GetComponentInChildren<TextMeshProUGUI>().text = success == 2? goodReply[Random.Range(0, goodReply.Length)] : badReply[Random.Range(0, badReply.Length)];
            OrderDone();
        }
        else if(status == Status.Hello)
        {
            status = Status.Order;
            cardSystem.running = true;
            doneButton.interactable = false;
            customerText.GetComponentInChildren<TextMeshProUGUI>().text = cardSystem.IsWOF()?customer.reverseText : customer.orderText;
            StartCoroutine(Move(customerText.transform, -1.125f, 0));
            coffeeMural.gameObject.SetActive(true);
            foodMural.gameObject.SetActive(true);
            coffeeMural.color = new Color(1, 1, 1, 1);
            if(cardSystem.IsWOF())
            coffeeMural.sprite = GetReverseCoffee(customer.coffee);
            else
            coffeeMural.sprite = GetCoffee(customer.coffee);
            foodMural.color = new Color(1, 1, 1, 1);
            switch (customer.food.name)
            {
                case "Cookie":
                    foodMural.sprite = foodIcon[6];
                    break;
                case "Croissant":
                    foodMural.sprite = foodIcon[7];
                    break;
                case "Cinnamon Roll":
                    foodMural.sprite = foodIcon[8];
                    break;
            }
            if(cardSystem.IsWOF())
                coffeeMural.GetComponentInChildren<TextMeshProUGUI>().text = (!customer.coffee.cold ? "Cold\n" : "Hot\n") + (!customer.coffee.cream ? "Cream\n" : "no Cream\n") + (!customer.coffee.choco ? "Chocolate\n" : "Cinnamon\n");
            else
                coffeeMural.GetComponentInChildren<TextMeshProUGUI>().text = (customer.coffee.cold ? "Cold\n" : "Hot\n") + (customer.coffee.cream ? "Cream\n" : "no Cream\n") + (customer.coffee.choco ? "Chocolate\n" : "Cinnamon\n");
            foodMural.GetComponentInChildren<TextMeshProUGUI>().text = customer.food.temp + " Cº\n" + customer.food.cookingTime + " Sec";

            StartCoroutine(Move(customerText.transform, 2, 3));
            StartCoroutine(Move(ghostText.transform, -3.5f, 0));
        }
    }

    public void OrderDone()
    {
        StartCoroutine(Move(customerText.transform, -0.7f, 0));
        
        actualCustomerGO.transform.DOMove(startPos, 2);
        doneButton.interactable = false;
        coffeeMural.gameObject.SetActive(false);
        foodMural.gameObject.SetActive(false);
        foodSpr.color = new Color(1, 1, 1, 0);
        coffeeSpr.color = new Color(1, 1, 1, 0);

        food = null;
        coffee = null;
    }

    IEnumerator Move(Transform target, float pos , float delay)
    {
        
        yield return new WaitForSeconds(delay);
        target.DOMoveY(pos, 0.5f);
    }
    public enum Status
    {
        MadelaineWalk, Tarot, MadelaineExit, NewCustomer, Hello, Order, Done, Exit, Close
    }    
}
