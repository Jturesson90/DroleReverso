using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComputerLevelToggle : MonoBehaviour
{

    public OthelloManager.ComputerLevelEnum computerLevel;
    private Toggle _toggle;
    bool _started;
    void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }
    void Start()
    {
        _toggle.isOn = OthelloManager.Instance.ComputerLevel == computerLevel ? true : false;
        _started = true;
    }
    public void OnRadioButtonValueChanged(bool val)
    {
        if (_toggle.isOn && _started)
        {
            print("Choosing " + computerLevel);
            OthelloManager.Instance.ComputerLevel = computerLevel;
        }
    }
}
