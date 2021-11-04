using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Food",menuName ="ScriptableObject/Food")]
public class Food : ScriptableObject
{
    public string name;
    public int cookingTime;
    public int temp;
}
