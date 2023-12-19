using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    GridData gridData;
    [SerializeField] private GameObject xObject;
    public GridData GridData => gridData;

    public void Initialize(int x, int y) 
    {
        gridData = new GridData(x, y);
    }

    public void DisplayX(bool value)
    {
        xObject.SetActive(value);
    }


}
