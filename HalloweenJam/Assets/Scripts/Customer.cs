using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Customer", menuName ="ScriptableObject/Customer")]
public class Customer : ScriptableObject
{
    [TextArea]
    public string orderText;
    [TextArea]
    public string reverseText;
    public Coffee coffee;
    public Food food;
}
