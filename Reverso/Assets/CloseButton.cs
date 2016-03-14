using UnityEngine;
using System.Collections;

public class CloseButton : MonoBehaviour
{

    public void OnCloseButtonClicked()
    {
        NavigationUtil.OnBackbuttonPressedInMenu();
    }
}
