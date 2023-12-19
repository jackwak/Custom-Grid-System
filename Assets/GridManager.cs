using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    #region Singleton
    public static GridManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [Header("Referances")]
    [SerializeField] Transform _gridParent, _gridCenter;
    [SerializeField] private CameraManager _cameraManager;

    [Header("Variables")]
    [Range(1, 10)]
    [SerializeField] private int gridsRowCount;
    [Range(1, 10)]
    [SerializeField] private int gridsColumnCount;
    GameObject[,] grids;

    [Range(1f,3f)]
    [SerializeField] private float distanceBetweenGrids;
    [SerializeField] private Vector3 gridsPosition = Vector3.zero;


    private void Start()
    {
        AlignGrids();
    }

    public void CheckForMatches(Grid grid)
    {
        List<Grid> matches = new List<Grid>();
        AddToMatches(grid, ref matches);

        if (matches.Count >= 3)
        {
            foreach (var item in matches)
            {
                item.DisplayX(false);
            }
        }
    }

    public void AddToMatches(Grid grid, ref List<Grid> matches)
    {
        if (!matches.Contains(grid))
        {
            matches.Add(grid);

            int addedRow = grid.GridData._rowOrder;
            int addedCol = grid.GridData._coloumnOrder;

            CheckDirections(addedRow, addedCol,ref matches);
        }
    }
    
    public void CheckDirections(int addedRowOrder, int addedColOrder, ref List<Grid> matches)
    {
        CheckGridDirection(addedRowOrder + 1, addedColOrder, ref matches);
        CheckGridDirection(addedRowOrder - 1, addedColOrder, ref matches);
        CheckGridDirection(addedRowOrder, addedColOrder + 1, ref matches);
        CheckGridDirection(addedRowOrder, addedColOrder - 1, ref matches);
    }

    public void CheckGridDirection(int newRowOrder, int newColOrder, ref List<Grid> matches)
    {
        if (IsWithInBounds(newRowOrder, newColOrder))
        {
            if (grids[newRowOrder, newColOrder].transform.GetChild(0).gameObject.activeSelf)
            {
                AddToMatches(grids[newRowOrder, newColOrder].GetComponent<Grid>(), ref matches);
            }
        }
    }

    bool IsWithInBounds(int newRowOrder, int newColOrder)
    {
        if (gridsRowCount <= newRowOrder || newRowOrder < 0 || gridsColumnCount <= newColOrder || newColOrder < 0)
        {
            return false;
        }
        return true;
    }

    public void AlignGrids()
    {
        grids = new GameObject[gridsRowCount,gridsColumnCount];

        for (int i = 0; i < gridsRowCount; i++)
        {
            for (int j = 0; j < gridsColumnCount; j++)
            {
                grids[i, j] = ObjectPool.Instance.GetPooledObject(0);
                grids[i, j].transform.position = new Vector3(i * distanceBetweenGrids, j * distanceBetweenGrids, 0);
                grids[i, j].transform.SetParent(_gridParent);
                grids[i, j].GetComponent<Grid>().Initialize(i, j);
            }
        }

        SetGridsCenterPosition(grids);
        SetCameraBounds(grids);
    }

    Vector3 CalculateOverallCenter(GameObject[,] grids)
    {
        Vector3 overallCenter = Vector3.zero;

        foreach (var obj in grids)
        {
            overallCenter += obj.transform.position;
        }

        overallCenter /= grids.Length;

        return overallCenter;
    }

    void SetGridsCenterPosition(GameObject[,] grids)
    {
        _gridCenter.position = CalculateOverallCenter(grids);
        _gridParent.SetParent(_gridCenter);
        _gridCenter.position = gridsPosition;
    }

    void SetCameraBounds(GameObject[,] grids)
    {
        Bounds bounds = _cameraManager.CalculateBounds(grids);
        _cameraManager.ExpandCameraView(bounds);
    }
}
