using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetApplicationFrameRate : MonoBehaviour
{
    [SerializeField] private int _frameRate = 60;
    private void Awake()
    {
        Application.targetFrameRate = _frameRate;
    }
}
