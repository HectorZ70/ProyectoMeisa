using UnityEngine;
using System.IO;
using SFB;
using UnityEngine.SceneManagement;

public class FileIO : MonoBehaviour
{
    public VirtualizedGrid grid;         
    public TextAsset fileToLoad;         
    public bool isTSV = true;
    private bool isLoadingFile = false;

    private void Awake()
    {
        if (grid == null)
        {
            grid = FindFirstObjectByType<VirtualizedGrid>();
            if (grid == null)
                Debug.LogError("No se encontró ningún VirtualizedGrid en la escena.");
        }
    }

    public void OpenFile()
    {
        if (isLoadingFile) return;

        if (grid == null)
        {
            grid = FindFirstObjectByType<VirtualizedGrid>();
            if (grid == null)
            {
                Debug.LogError("grid sigue siendo null al abrir archivo.");
                return;
            }
        }

        isLoadingFile = true;

        var extensions = new[]
        {
        new ExtensionFilter("TSV files", "tsv"),
        new ExtensionFilter("CSV files", "csv"),
        new ExtensionFilter("All files", "*"),
    };

        StandaloneFileBrowser.OpenFilePanelAsync("Seleccionar archivo", "", extensions, false, (paths) =>
        {
            isLoadingFile = false;

            if (paths.Length > 0 && File.Exists(paths[0]))
            {
                string text = File.ReadAllText(paths[0]);

                GridLoadBuffer.RawFileData = text;
                GridLoadBuffer.IsTSV = Path.GetExtension(paths[0]).ToLower() == ".tsv";

                SceneManager.LoadScene("ListScene");
            }
        });
    }

    public void SaveFile()
    {
        if (grid == null)
        {
            grid = FindFirstObjectByType<VirtualizedGrid>();

            if (grid == null)
            {
                Debug.LogError("No se encontró VirtualizedGrid para guardar");
                return;
            }
        }
        GridSaveData saveData = new GridSaveData();

        foreach (var kvp in grid.cellData)
        {
            GridCellSaveData cell = new GridCellSaveData();
            cell.row = kvp.Key.x;
            cell.column = kvp.Key.y;
            cell.text = kvp.Value;
            cell.isHighlighted = grid.highlightedColumns.Contains(kvp.Key.y);

            saveData.cells.Add(cell);
        }

        string json = JsonUtility.ToJson(saveData, true);

        string path = StandaloneFileBrowser.SaveFilePanel(
            "Guardar como JSON",
            "",
            "grid_data",
            "json"
        );

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            Debug.Log($"Datos guardados en: {path}");
        }
    }
}