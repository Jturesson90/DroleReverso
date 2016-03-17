using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class MenuCircle : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public float pushSpeed = 1f;
    public Vector2 speed;

    public Color blackColor;
    public Color whiteColor;



    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    // Use this for initialization
    void Start()
    {
        Push();
    }



    private void Push()
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(Random.insideUnitCircle * pushSpeed, ForceMode2D.Impulse);
    }

    public void GiveBlackColor()
    {
        GetComponent<SpriteRenderer>().color = blackColor;
    }

    public void GiveWhiteColor()
    {
        GetComponent<SpriteRenderer>().color = whiteColor;
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "CirclesWall")
        {
            print("New PUSH!");
            Push();
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.name == "CirclesWall")
            Push();

    }
}
