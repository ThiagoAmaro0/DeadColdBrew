using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorkStation : MonoBehaviour
{
    [SerializeField] Transform view;
    [SerializeField] Transform workCamera;
    static Vector3 defaultPos;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = workCamera.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            workCamera.transform.DOMove(view.position,1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            workCamera.transform.DOMove(defaultPos, 1);
        }
    }
    public void SetActive(bool value)
    {
        GetComponent<Collider>().enabled = value;
        if (!value)
            workCamera.transform.DOMove(defaultPos, 1);
    }
}
