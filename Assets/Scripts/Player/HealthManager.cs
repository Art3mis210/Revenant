using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    int Value;
    Image Bar;
    Image Symbol;
    public static HealthManager Reference
    {
        get;
        set;
    }
    private void Start()
    {
        Reference = this;
        Bar = GetComponent<Image>();
        Symbol = transform.GetChild(1).GetComponent<Image>();
    }
    public void UpdateHealth(int Health)
    {
        Value = Health;
        if (Value > 0)
            Bar.fillAmount = Health/100f;
        else
            Bar.fillAmount = 0;
        if(Health>75)
        {
            Bar.color = Color.green;
            Symbol.color = Color.green;
        }
        else if(Health>50)
        {
            Bar.color = Color.yellow;
            Symbol.color = Color.yellow;
        }
        else if (Health > 25)
        {
            Bar.color = new Color(255, 140, 0,255);
            Symbol.color = new Color(255, 140, 0,255);
        }
        else if(Health>0)
        {
            Bar.color = Color.red;
            Symbol.color = Color.red; 
        }
        else
        {
            Bar.color = Color.gray;
            Symbol.color = Color.gray;
        }
    }
}
