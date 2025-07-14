using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GridCellSaveData
{
    public int row;
    public int column;
    public string text;
    public bool isHighlighted;
}

[System.Serializable]
public class GridSaveData
{
    public List<GridCellSaveData> cells = new List<GridCellSaveData>();
}

public static class GridLoadBuffer
{
    public static GridSaveData DataToLoad;
    public static string RawFileData;
    public static bool IsTSV;
}