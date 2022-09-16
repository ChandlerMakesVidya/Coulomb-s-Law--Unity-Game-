using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Charge : MonoBehaviour
{
    [Tooltip("Is this charge negative?")]
    public bool negative;
    [Tooltip("The value of this charge, affects the force exerted on it by the mouse.")]
    public float charge;

    private Rigidbody2D rb;

    public void SwitchSign()
    {
        if (!negative)
        {
            SetNegative();
        }
        else
        {
            SetPositive();
        }
    }

    public void SetNegative()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        TMP_Text sign = GetComponentInChildren<TMP_Text>();

        sr.color = Color.cyan;
        sign.text = "-";
        negative = true;
    }

    public void SetPositive()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        TMP_Text sign = GetComponentInChildren<TMP_Text>();

        sr.color = Color.red;
        sign.text = "+";
        negative = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (negative)
        {
            SetNegative();
        }
    }

    private void Update()
    {
        Level lv = GetComponentInParent<Level>();
        if(lv.uniformField != Vector2.zero)
        {
            Vector2 dir = new Vector2(
                Mathf.Cos(lv.uniformField.x * Mathf.Deg2Rad), 
                Mathf.Sin(lv.uniformField.x * Mathf.Deg2Rad));
            rb.AddForce(dir * lv.uniformField.y);
        }
        //print(mousePos);
        if (Input.GetMouseButton(0) && GameManager.gameState == GameManager.GameState.Playing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 dir = position - mousePos;
            if (negative) dir = -dir;
            float distance = Vector2.Distance(transform.position, mousePos);
            float r = distance > 0.1f ? distance : 0.1f;
            float magnitude = charge / (r * r);
            rb.AddForce(dir * magnitude);
        }
    }
}
