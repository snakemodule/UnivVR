using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Widget : MonoBehaviour
{

    public GameObject Controller;
    public bool fixedPos;

    enum mode { none, rotate, translate, roll, pan };
    mode currentMode = mode.none;

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

    public void setTransparent()
    {
        for (int i = 0; i < 6; i++)
        {
            transparentChildIndex(i);
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

    public void hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
    }

    public void highlightLeft()
    {
        highlightChildIndex(0);
    }

    public void highlightRight()
    {
        highlightChildIndex(1);
    }

    public void highlightUp()
    {
        highlightChildIndex(2);
    }

    public void highlightDown()
    {
        highlightChildIndex(3);
    }

    public void highlightForward()
    {
        highlightChildIndex(4);
    }

    public void highlightBackward()
    {
        highlightChildIndex(5);
    }

    public void offColorLeft()
    {
        offColorChildIndex(0);
    }

    public void offColorRight()
    {
        offColorChildIndex(1);
    }

    public void offColorUp()
    {
        offColorChildIndex(2);
    }

    public void offColorDown()
    {
        offColorChildIndex(3);
    }

    public void offColorForward()
    {
        offColorChildIndex(4);
    }

    public void offColorBackward()
    {
        offColorChildIndex(5);
    }

    private void highlightChildIndex(int index)
    {
        transform.GetChild(index).GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    }

    private void offColorChildIndex(int index)
    {
        transform.GetChild(index).GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    }

    private void transparentChildIndex(int index)
    {
        Color transparentBlue = new Color(0, 0, 1, 0.5f);
        transform.GetChild(index).GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", transparentBlue);
    }

}
