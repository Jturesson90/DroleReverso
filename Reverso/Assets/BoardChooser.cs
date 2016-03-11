using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardChooser : MonoBehaviour
{

    public List<GameObject> boards;
    // Use this for initialization
    void Awake()
    {
        ChooseBoard(OthelloManager.ChosenBoard);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChooseBoard(int id)
    {
        bool choseBoard = false;
        for (int i = 0; i < boards.Count; i++)
        {
            if (id == i)
            {
                boards[i].SetActive(true);
                choseBoard = true;
            }
            else
            {
                boards[i].SetActive(false);
            }
        }

        if (!choseBoard && boards.Count > 0)
        {
            boards[0].SetActive(true);
        }
    }
}
