using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSwitch : MonoBehaviour
{
    public enum SwitchMode
    {
        Switch, Positive, Negative
    }

    public SwitchMode switchMode;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        switch (switchMode)
        {
            case SwitchMode.Switch:
                Charge charge = FindObjectOfType<Charge>();
                if (!charge.negative) sr.color = Color.cyan;
                else sr.color = Color.red;
                break;
            case SwitchMode.Positive:
                sr.color = Color.red;
                break;
            case SwitchMode.Negative:
                sr.color = Color.cyan;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Charge")
        {
            Charge charge = collision.gameObject.GetComponent<Charge>();
            switch (switchMode)
            {
                case SwitchMode.Switch:
                    if (!charge.negative) sr.color = Color.red;
                    else sr.color = Color.cyan;
                    charge.SwitchSign();
                    break;
                case SwitchMode.Positive:
                    charge.SetPositive();
                    break;
                case SwitchMode.Negative:
                    charge.SetNegative();
                    break;
            }
        }
    }
}
