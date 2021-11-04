using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] Transform meshObj;
    float rot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = 180;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x == 0 && rot != 180) 
        {
            rot = 180;
            meshObj.DORotate(new Vector3(0, rot, 0), 0.5f);
        }
        else if (x == 1 && rot != 90)
        {
            rot = 90;
            meshObj.DORotate(new Vector3(0, rot, 0), 0.5f);
        }
        else if (x == -1 && rot != -90)
        {
            rot = -90;
            meshObj.DORotate(new Vector3(0, rot, 0), 0.5f);
        }

        Vector2 velocity = new Vector2(x,y).normalized * Time.fixedDeltaTime * speed;
        rb.velocity = velocity;
    }
}
