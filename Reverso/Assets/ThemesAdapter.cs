using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThemesAdapter : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        //ResetAllToggles();
        PlayerPrefs.SetInt("RADIO_BUTTON0", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ResetAllToggles()
    {

        for (int ID = 1; ID < 4; ID++)
        {
            PlayerPrefs.SetInt("RADIO_BUTTON" + ID, 1);
        }
    }
}
