using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public CoalMarketTrend coalmarkettrend;
    public TextMeshProUGUI coalmoney;
    public TextMeshProUGUI coal;
    public TextMeshProUGUI text;
    
    private void Start()
    {
        text.text = (" ");
        coalpanel.SetActive(false);
        Mechanicalpanel.SetActive(false);
        Ironpanel.SetActive(false);
        Lithiumpanel.SetActive(false);
        Specialpanel.SetActive(false);
    }

    private void Update()
    {
        coalmoney.text = coalpurchaseprice.ToString();
        coal.text = coalsaleprice.ToString();
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
            text.text = ("Trade sucess");
            Money.counter -= coalpurchaseprice;
            Coal.counterC += coalpurchaseamount;
            coalpurchaseprice += 1f;
        }
        else
        {
            text.text = ("Trade fail");
        }
    }
    
    public void coalsale()
    {
        if (Coal.counterC>=coalsaleprice)
        {
            text.text = ("Trade sucess");
            Coal.counterC -= coalsaleprice;
            Money.counter += coalsaleamount;
            coalsaleprice -= 1f;
            coalpurchaseprice -= 1f;
        }
        else
        {
            text.text = ("Trade fail");
        }
    }
}
