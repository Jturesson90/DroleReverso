using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class RadioToggle : MonoBehaviour
{


    Toggle _toggle;
    public Image backCircle;
    private Color _backCircleColorOn;
    private Color _backCircleColorOff;
    [Range(0, 1)]
    public float backCircleGradient;
    public Image toggleBackgroundImage;
    public Color normalColor;


    bool _started;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _backCircleColorOn = new Color(normalColor.r, normalColor.g, normalColor.b, backCircleGradient);
        _backCircleColorOff = new Color(255f, 255f, 255f, backCircleGradient);

    }
    void Start()
    {
        _started = true;
        SetColorToCircle();
        SetRadioButtonNormalColor();
    }

    public void OnValueChanged(bool val)
    {
        if (!_started) return;
        SetColorToCircle();
        SetRadioButtonNormalColor();
    }

    private void SetColorToCircle()
    {
        if (_toggle.isOn)
        {
            backCircle.color = _backCircleColorOn;

        }
        else
        {

            backCircle.color = _backCircleColorOff;
        }

    }

    private void SetRadioButtonNormalColor()
    {
        if (_toggle.isOn)
        {
            toggleBackgroundImage.color = normalColor;
        }
        else
        {
            Color whiteColorWithAlpha = Color.white;
            whiteColorWithAlpha.a *= 0.7f;
            toggleBackgroundImage.color = whiteColorWithAlpha;
        }

    }
}
