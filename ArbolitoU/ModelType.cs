using System.Collections.Generic;
using CodeWalker.GameFiles;

namespace ArbolitoU;

public class ModelType
{
    public List<YdrFile> YdrFiles { get; set; } = new();
    public List<YddFile> YddFiles { get; set; } = new();
    public List<YftFile> YftFiles { get; set; } = new();
}