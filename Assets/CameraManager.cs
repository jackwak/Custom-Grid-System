using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private float _padding = 2f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void SetCameraPosition(Vector3 centerOfCells)
    {
        _camera.transform.position = centerOfCells;
    }

    public Bounds CalculateBounds(GameObject[,] cells)
    {
        Bounds bounds = new Bounds(cells[0,0].transform.position, Vector3.zero);

        foreach (GameObject cell in cells)
        {
            bounds.Encapsulate(cell.transform.position);
        }

        return bounds;
    }

    public void ExpandCameraView(Bounds bounds)
    {
        float requiredSize = Mathf.Max(bounds.size.x, bounds.size.y) / 2f + _padding;
        float currentSize = _camera.orthographicSize;
        
        // Kamera boyutunu dinamik olarak ayarla
        _camera.orthographicSize = requiredSize;
    }
}
