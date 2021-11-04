using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachine : MonoBehaviour
{
    Coffee coffee;
    [SerializeField] WorkStation workStation;
    [SerializeField] Button doneButton;
    [SerializeField] int price;
    [SerializeField] float processTime;
    float actualProcessTime;
    bool processing;
    [SerializeField] Animator coldAnim, chocoAnim, creamAnim, machineAnim;
    // Start is called before the first frame update
    void Start()
    {
        actualProcessTime = processTime;
        coffee = new Coffee();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleCream()
    {
        coffee.cream = !coffee.cream;
        creamAnim.SetBool("Up", coffee.cream);
    }

    public void ToggleCold()
    {
        coffee.cold = !coffee.cold;
        coldAnim.SetBool("Up", coffee.cold);
    }

    public void ToggleChoco()
    {
        coffee.choco = !coffee.choco;
        chocoAnim.SetBool("Up", coffee.choco);
    }

    public void Process()
    {
        if (!processing)
        {
            StartCoroutine(StartProcess());
        }
        IEnumerator StartProcess()
        {
            processing = true;
            AudioManager.Play("CoffeeMachine");
            machineAnim.SetBool("On", true);
            doneButton.interactable = false;
            workStation.SetActive(false);
            GameManager.instance.money -= price;
            yield return new WaitForSeconds(actualProcessTime);
            processing = false;
            AudioManager.Stop("CoffeeMachine");
            AudioManager.Play("Pires");
            workStation.SetActive(true);
            machineAnim.SetBool("On", false);
            doneButton.interactable = true;
            GameManager.instance.SetCoffee(coffee);
        }
    }
}
