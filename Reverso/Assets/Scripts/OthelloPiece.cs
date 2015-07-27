using UnityEngine;
using System.Collections;

public enum BrickColor
{
	White = 0,
	Black = 1,
	Empty = 2,
	Hint
}
;

//For Animator Black is 1 and White is 0

public class OthelloPiece : MonoBehaviour
{

	public BrickColor brickColor;
	private Animator anim;
	private const string
		ANIMATOR_BRICK = "Brick";




	public int x, y;

	void Awake ()
	{
		anim = GetComponent<Animator> ();
	}
	// Use this for initialization
	void Start ()
	{

	}

	void FixedUpdate ()
	{
		CheckAnimations ();
	}
	private void CheckAnimations ()
	{
		if (brickColor == BrickColor.White) {
			anim.SetInteger (ANIMATOR_BRICK, 0);
		} else if (brickColor == BrickColor.Black) {
			anim.SetInteger (ANIMATOR_BRICK, 1);
		} else if (brickColor == BrickColor.Empty) {
			anim.SetInteger (ANIMATOR_BRICK, 2);
		}
	}

}
