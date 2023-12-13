using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] Transform _gridParent;
    [SerializeField] Transform _gridCenter;
    [SerializeField] private CameraManager _cameraManager;

    [Header("Variables")]
    public int[,] GridCount = new int[3,5];

    [Range(1, 10)]
    [SerializeField] int gridsRowCount, gridsColumnCount;

    [Range(1f,3f)]
    [SerializeField] private float distanceBetweenGrids;
    [SerializeField] private Vector3 gridsPosition = Vector3.zero;

    private void Start()
    {
        AlignGrids();
        
    }

    public void AlignGrids()
    {
        GameObject[,] grids = new GameObject[gridsRowCount,gridsColumnCount];

        for (int i = 0; i < gridsRowCount; i++)
        {
            for (int j = 0; j < gridsColumnCount; j++)
            {
                grids[i, j] = ObjectPool.Instance.GetPooledObject(0);
                grids[i, j].transform.position = new Vector3(i * distanceBetweenGrids, j * distanceBetweenGrids, 0);
                grids[i, j].transform.SetParent(_gridParent);
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
