using UnityEngine;
using System.Collections;
using System;

public enum BrickColor
{
    White = 0,
    Black = 1,
    Empty = 2,
    Hint
}
;

//For Animator Black is 1 and White is 0

public class OthelloPiece : MonoBehaviour, ICloneable
{

    public BrickColor brickColor;
    private Animator anim;
    private const string
        ANIMATOR_BRICK = "Brick";
    private Renderer rend;

    public bool ShouldFlash
    {
        get;
        set;
    }

    public int x, y;
    private Color firstWhiteColor;
    private Color firstBlackColor;
    float time = 0f;
    Color newWhiteColor, newBlackColor;
    void Awake()
    {
        newWhiteColor = new Color();
        newBlackColor = new Color();
        anim = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        firstBlackColor = rend.materials[0].color;
        firstWhiteColor = rend.materials[1].color;

    }
    void Update()
    {
        if (ShouldFlash)
        {

            time += Time.deltaTime;
            newWhiteColor.r = Mathf.PingPong(time, 1f);
            newWhiteColor.g = Mathf.PingPong(time, 1f);
            newWhiteColor.b = Mathf.PingPong(time, 1f);

            newBlackColor.r = 1 - Mathf.PingPong(time, 1f);
            newBlackColor.g = 1 - Mathf.PingPong(time, 1f);
            newBlackColor.b = 1 - Mathf.PingPong(time, 1f);

            rend.materials[0].color = newWhiteColor;
            rend.materials[1].color = newBlackColor;


        }
        else
        {
            time = 0f;
            rend.materials[0].color = firstBlackColor;
            rend.materials[1].color = firstWhiteColor;
        }



    }

    void FixedUpdate()
    {
        CheckAnimations();
    }
    private void CheckAnimations()
    {
        if (brickColor == BrickColor.White)
        {
            anim.SetInteger(ANIMATOR_BRICK, 0);
        }
        else if (brickColor == BrickColor.Black)
        {
            anim.SetInteger(ANIMATOR_BRICK, 1);
        }
        else if (brickColor == BrickColor.Empty)
        {
            anim.SetInteger(ANIMATOR_BRICK, 2);
        }
        else if (brickColor == BrickColor.Hint)
        {
            anim.SetInteger(ANIMATOR_BRICK, 3);
        }
    }

    public object Clone()
    {
        OthelloPiece newOthelloPiece = (OthelloPiece)this.MemberwiseClone();

        return newOthelloPiece;
    }
}
