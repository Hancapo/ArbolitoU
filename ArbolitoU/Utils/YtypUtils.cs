using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeWalker.GameFiles;

namespace ArbolitoU.Utils;

public class YtypUtils
{
    public static List<ArchetypeElement> GetArchetypeElements(IEnumerable<string> ytypFiles)
    {
        List<ArchetypeElement> archetypeNames = new();

        Parallel.ForEach(ytypFiles, ytyp =>
        {
            ArchetypeElement archetypeElement = new();
            var ytypFile = new YtypFile();
            ytypFile.Load(File.ReadAllBytes(ytyp));
            
            archetypeElement.YtypName = Path.GetFileNameWithoutExtension(ytyp);
            
            List<MetaHash> metaHashes = new();
            
            metaHashes.AddRange(ytypFile.AllArchetypes.Where(archetype => archetype.Type is MetaName.CBaseArchetypeDef or MetaName.CTimeArchetypeDef).Select(archetype => archetype.Hash));

            if (metaHashes.Count <= 0) return;
            archetypeElement.archetypeNames = metaHashes;
            archetypeNames.Add(archetypeElement);

        });
        return archetypeNames;
    }
    
    public static List<ArchetypeElement> GetArchetypeElements(string textFile)
    {
        List<ArchetypeElement> archetypesNames = new();
        ArchetypeElement archetypeElement = new();
        List<MetaHash> fileElements = new();

        Parallel.ForEach(File.ReadAllLines(textFile), item =>
        {
            fileElements.Add(JenkHash.GenHash(item.ToLower().Trim()));

        });

        archetypeElement.archetypeNames = fileElements;
        archetypeElement.YtypName = Path.GetFileNameWithoutExtension(textFile);
        archetypesNames.Add(archetypeElement);
        return archetypesNames;
    }
}