using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    Type type;
    [SerializeField] Vector2 RangeLightDelay;
    [SerializeField] GameObject fadeObject, timerObject;
    float time;
    public bool running;


    public void RandomCard()
    {
        Type[] types = new Type[]{ Type.Sun,Type.Tower,Type.WOF };
        type = types[Random.Range(0, 3)];
        switch (type)
        {
            case Type.Tower:
                timerObject.SetActive(true);
                time = Time.deltaTime + Random.Range(RangeLightDelay.x, RangeLightDelay.y);
                break;
            case Type.Sun:
                timerObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (running) {
            switch (type)
            {
                case Type.Tower:
                    TowerLoop();
                    break;
                default:
                    break;
            }
        }
    }

    void TowerLoop()
    {
        if (time < Time.time && !fadeObject.active)
        {
            AudioManager.Play("Light");
            fadeObject.SetActive(true);
        }
    }
    public void FixLight()
    {
        if (fadeObject.active)
        {
            AudioManager.Play("Light");
            fadeObject.SetActive(false);
            time = Time.time + Random.Range(RangeLightDelay.x, RangeLightDelay.y);
        }
    }
    public bool IsWOF()
    {
        return type == Type.WOF;
    }

    public Type GetCard()
    {
        return type;
    }
    public enum Type
    {
        None, Tower, Sun, WOF
    }
}
