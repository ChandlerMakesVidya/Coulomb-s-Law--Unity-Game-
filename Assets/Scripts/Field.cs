using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [Tooltip("Angle and magnitude of this field. X value is angle, Y value is magnitude. >700 is stronger than the charge can be moved by the player.")]
    public Vector2 angleAndMagnitude;
    private BoxCollider2D box;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Charge")
        {
            Vector2 dir = new Vector2(
                Mathf.Cos(angleAndMagnitude.x * Mathf.Deg2Rad),
                Mathf.Sin(angleAndMagnitude.x * Mathf.Deg2Rad));
            if (collision.gameObject.GetComponent<Charge>().negative) dir = -dir;
            collision.GetComponent<Rigidbody2D>().AddForce(dir * angleAndMagnitude.y);
        }
    }
}
