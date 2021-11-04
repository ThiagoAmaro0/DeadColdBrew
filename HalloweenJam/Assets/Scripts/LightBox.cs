using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBox : MonoBehaviour
{
    [SerializeField] CardSystem cardSystem;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name  == "Player")
        {
            cardSystem.FixLight();
        }
    }
}
