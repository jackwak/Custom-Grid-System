using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public int _rowOrder { get; private set; }
    public int _coloumnOrder { get; private set; }

    public GridData(int rowOrder, int coloumnOrder)
    {
        _rowOrder = rowOrder;
        _coloumnOrder = coloumnOrder;
    }

}
