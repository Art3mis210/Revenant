using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    float Value;
    Image Bar;
    Image Symbol;
    public static StaminaManager Reference
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
    public void UpdateStamina(float Stamina)
    {
        Value = Stamina;
        if (Value > 0)
            Bar.fillAmount = Value / 100f;
        else
            Bar.fillAmount = 0;
        if (Value > 75)
        {
            Bar.color = Color.green;
            Symbol.color = Color.green;
        }
        else if (Value > 50)
        {
            Bar.color = Color.yellow;
            Symbol.color = Color.yellow;
        }
        else if (Value > 25)
        {
            Bar.color = new Color(255, 140, 0, 255);
            Symbol.color = new Color(255, 140, 0, 255);
        }
        else if (Value > 0)
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
