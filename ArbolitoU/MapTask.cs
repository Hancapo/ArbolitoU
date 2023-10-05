using System.Collections.Generic;

namespace ArbolitoU;

public class MapTask
{
    public string FilePath { get; set; }
    public string? FileName { get; }
    public List<uint> EntityHashes { get; set; }
    
    public MapTask(string filePath)
    {
        FilePath = filePath;
        FileName = System.IO.Path.GetFileName(filePath);
        EntityHashes = new List<uint>();
    }
}