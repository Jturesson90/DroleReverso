using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleImageHandler : MonoBehaviour
{
    Toggle toggle;

    public Color OffColor;
    public Color OnColor;
    public Sprite OffImage;
    public Sprite OnImage;
    private Color _backCircleColorOn;
    private Color _backCircleColorOff;
    public Color NormalColor;
    public Graphic backgroundGraphic;

    public Image backCircle;
    [Range(0, 1)]
    public float backCircleGradient;

    // Use this for initialization
    void Awake()
    {
        toggle = GetComponent<Toggle>();
        _backCircleColorOn = new Color(NormalColor.r, NormalColor.g, NormalColor.b, backCircleGradient);
        _backCircleColorOff = new Color(255f, 255f, 255f, backCircleGradient);
    }
    void Start()
    {
        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleChanged()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        SetColorToCircle();

        var image = backgroundGraphic.GetComponent<Image>();
        if (toggle.isOn)
        {

            image.sprite = OnImage;
            image.color = OnColor;
        }
        else
        {
            image.sprite = OffImage;
            image.color = OffColor;
        }
    }

    private void SetColorToCircle()
    {
        if (toggle.isOn)
        {
            backCircle.color = _backCircleColorOn;

        }
        else
        {

            backCircle.color = _backCircleColorOff;
        }
    }
}
