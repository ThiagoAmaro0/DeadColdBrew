using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Oven : MonoBehaviour
{
    Food recipe, food;
    [SerializeField] int price;
    int temp;
    [SerializeField] TextMeshProUGUI timerText, tempText;    
    [SerializeField] SpriteRenderer foodSpr;    
    [SerializeField]Animator miniAnim;
    Animator anim;
    float timer;
    bool start;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        food = new Food();
        recipe = new Food();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            timer += Time.deltaTime;
        }
        foodSpr.sprite = GameManager.instance.GetFood(food, recipe);
        timerText.text = Mathf.FloorToInt(timer / 60).ToString("d2")+":" +((int)(timer%60)).ToString("d2");
    }
    public void Process()
    {
        if (!start && recipe.name != "")
        {
            timer = 0;
            food.temp = temp;
            food.name = recipe.name;
            start = true;
            AudioManager.Play("Oven");
            AudioManager.Play("Timer");
            GameManager.instance.money -= price;
        }
        else if(start)
        {
            AudioManager.Stop("Timer");
            AudioManager.Stop("Oven");
            food.cookingTime = (int)timer;
            start = false;
            GameManager.instance.SetFood(food, recipe);
            food = new Food();
        }
        anim.SetBool("On", start);
        miniAnim.SetBool("On", start);
    }
    public void ChangeTemp(int value)
    {
        temp = Mathf.Clamp(temp + value, 0, 300);
        tempText.text = temp + " Cº";
    }
    public void SetRecipe(Food _recipe)
    {
        if (!start)
        {
            recipe = _recipe;
            food.temp = temp;
            food.name = recipe.name;
            foodSpr.sprite = foodSpr.sprite = GameManager.instance.GetFood(food, _recipe); ;
        }
    }
}
