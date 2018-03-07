using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Widget : MonoBehaviour
{

    public GameObject Controller;
    public bool fixedPos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!fixedPos)
        {
            transform.position = Controller.transform.position - new Vector3(0.08f, 0, 0);
        }

    }

    public void rotateMode()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
    }

    public void translateMode()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(5).gameObject.SetActive(true);
    }

    public void rollMode()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
    }

    public void panMode()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(5).gameObject.SetActive(true);
    }

    public void highlightLeft()
    {
        highlightIndex(0);
    }

    public void highlightRight()
    {
        highlightIndex(1);
    }

    public void highlightUp()
    {
        highlightIndex(2);
    }

    public void highlightDown()
    {
        highlightIndex(3);
    }

    public void highlightForward()
    {
        highlightIndex(4);
    }

    public void highlightBackward()
    {
        highlightIndex(5);
    }

    public void offColorLeft()
    {
        offColorIndex(0);
    }

    public void offColorRight()
    {
        offColorIndex(1);
    }

    public void offColorUp()
    {
        offColorIndex(2);
    }

    public void offColorDown()
    {
        offColorIndex(3);
    }

    public void offColorForward()
    {
        offColorIndex(4);
    }

    public void offColorBackward()
    {
        offColorIndex(5);
    }

    private void highlightIndex(int index)
    {
        transform.GetChild(index).GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        //GameObject.Find("Sphere").GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    }

    private void offColorIndex(int index)
    {
        transform.GetChild(index).GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    }

}
