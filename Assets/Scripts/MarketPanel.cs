using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MarketPanel : MonoBehaviour
{
    public GameObject marketpanel;
    public GameObject coalpanel;
    public GameObject Mechanicalpanel;
    public GameObject Ironpanel;
    public GameObject Lithiumpanel;
    public GameObject Specialpanel;
    public float coalpurchaseprice = 5f;
    public float coalpurchaseamount = 10f;
    public float coalsaleprice = 4f;
    public float coalsaleamount = 3f;
    public Money Money;
    public Coal Coal;
    public GameObject coaltrend;
    private void Start()
    {
        coalpanel.SetActive(false);
        Mechanicalpanel.SetActive(false);
        Ironpanel.SetActive(false);
        Lithiumpanel.SetActive(false);
        Specialpanel.SetActive(false);
    }

    public void closepanel()
    {
        marketpanel.SetActive(false);
    }

    public void togglecoal()
    {
        coalpanel.SetActive(!coalpanel.activeSelf);
        coaltrend.SetActive(!coaltrend.activeSelf);
    }
    
    public void togglemec()
    {
        Mechanicalpanel.SetActive(!Mechanicalpanel.activeSelf);
    }
    
    public void toggleiron()
    {
        Ironpanel.SetActive(!Ironpanel.activeSelf);
    }
    
    public void togglelithium()
    {
        Lithiumpanel.SetActive(!Lithiumpanel.activeSelf);
    }
    
    public void togglespecial()
    {
        Specialpanel.SetActive(!Specialpanel.activeSelf);
    }

    public void coalpurchase()
    {
        if (Money.counter >= coalpurchaseprice)
        {
            Money.counter -= coalpurchaseprice;
            Coal.counterC += coalpurchaseamount;
        }
    }
    
    public void coalsale()
    {
        if (Coal.counterC>=coalsaleprice)
        {
            Coal.counterC -= coalsaleprice;
            Money.counter += coalsaleamount;
            Debug.LogError("1");
        }
    }
}
