using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{   
    public static CellManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    [Header("Referances")]
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _cellSize;
    [SerializeField] private Transform _cellParent;
    [SerializeField] private CameraManager _cameraManager;

    [Header("Variables")]
    [SerializeField] private float _cellOffset;
    private int[,] _cellPosition = new int[5, 3];
    public GameObject[,] cells;

    public List<GameObject> x;

    public int[,] CellPosition 
    {
        get
        {
            return _cellPosition;
        }
    }


    private void Start()
    {
        AlingCells();

    }

    void AlingCells()
    {
        float spaceBetweenCells = _cellSize.localScale.x + _cellOffset;
        int rowCount = _cellPosition.GetLength(0);
        int columnCount = _cellPosition.GetLength(1);
        cells = new GameObject[rowCount , columnCount];


        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                cells[i,j] = Instantiate(_cellPrefab, new Vector2(i * spaceBetweenCells,j * spaceBetweenCells), Quaternion.identity, _cellParent);
                cells[i,j].GetComponent<Cell>().rowOrder = i;
                cells[i,j].GetComponent<Cell>().columnOrder = j;
            }
        }
        Vector3 centerOfCells = CalculateOverallCenter(cells);
        _cameraManager.SetCameraPosition(centerOfCells);
        Bounds bounds = _cameraManager.CalculateBounds(cells);
        _cameraManager.ExpandCameraView(bounds);
    }




    Vector3 CalculateOverallCenter(GameObject[,] objects)
    {
        Vector3 overallCenter = Vector3.zero;

        foreach (GameObject obj in objects)
        {
            Vector3 objCenter = GetObjectCenter(obj);

            overallCenter += objCenter;
        }

        overallCenter /= objects.Length;

        return overallCenter;
    }

    Vector3 GetObjectCenter(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();

        if (objRenderer != null)
        {
            Bounds bounds = objRenderer.bounds;
            return bounds.center;
        }
        else
        {
            Debug.LogError("Renderer bileþeni bulunamadý!");
            return Vector3.zero;
        }
    }
}
