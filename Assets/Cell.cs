using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _xPrefab;

    int rowCount;
    int columnCount;

    public int rowOrder;
    public int columnOrder;
    

    private void Start()
    {
        Initialize();
    }

    private void OnMouseDown()
    {
        CellManager.Instance.x.Add(CellManager.Instance.cells[rowOrder, columnOrder]);
        transform.GetChild(0).gameObject.SetActive(true);
        CheckCellAllDirection();
    }

    void Initialize()
    {
        rowCount = CellManager.Instance.CellPosition.GetLength(0);
        columnCount = CellManager.Instance.CellPosition.GetLength(1);
    }

    void CheckCellAllDirection()
    {
        CheckCellDirection(1, 0);
        CheckCellDirection(-1, 0);
        CheckCellDirection(0, 1);
        CheckCellDirection(0, -1);
    }

    void CheckCellDirection(int xDirection, int yDirection)
    {
        if (!IsInBounds(xDirection, yDirection)) return;

        GameObject otherCell = CellManager.Instance.cells[rowOrder + xDirection, columnOrder + yDirection];

        if (CellManager.Instance.x.Contains(otherCell)) return;

        if (otherCell.transform.childCount > 0)
        {
            CellManager.Instance.x.Add(otherCell.gameObject);
            otherCell.GetComponent<Cell>().CheckCellAllDirection();
        }

        if (CellManager.Instance.x.Count >= 3)
        {
            foreach (var item in CellManager.Instance.x)
            {
                item.GetComponent<Cell>().SetActiveFalseChild();
            }
        }
    }

    bool IsInBounds(int xDirection, int yDirection)
    {
        if (rowOrder + xDirection < rowCount && rowOrder + xDirection >= 0 &&
            columnOrder + yDirection < columnCount && columnOrder + yDirection >= 0)
        {
            return true;
        }
        return false;
    }

    void SetActiveFalseChild()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
