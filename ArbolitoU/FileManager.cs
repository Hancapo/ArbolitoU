using System.Collections.Generic;
using System.Linq;
using Avalonia.Platform.Storage;

namespace ArbolitoU;

public class FileManager
{
    public readonly List<string> SupportedFormats =
        ["*.ydr", "*.ytyp", "*.ymap", "*.ytd", "*.ydd", "*.dat", "*.txt", "*.ynv"];

    public List<string> ydrList = [];
    public List<string> ytypList = [];
    public List<string> ymapList = [];
    public List<string> ytdList = [];
    public List<string> yddList = [];
    public List<string> datList = [];
    public List<string> txtList = [];
    public List<string> ynvList = [];


    public void LoadFiles(List<string> selectedFiles)
    {
        foreach (string file in selectedFiles)
        {
            if (file.EndsWith(".ydr"))
            {
                ydrList.Add(file);
            }
            else if (file.EndsWith(".ytyp"))
            {
                ytypList.Add(file);
            }
            else if (file.EndsWith(".ymap"))
            {
                ymapList.Add(file);
            }
            else if (file.EndsWith(".ytd"))
            {
                ytdList.Add(file);
            }
            else if (file.EndsWith(".ydd"))
            {
                yddList.Add(file);
            }
            else if (file.EndsWith(".dat"))
            {
                datList.Add(file);
            }
            else if (file.EndsWith(".txt"))
            {
                txtList.Add(file);
            }
            else if (file.EndsWith(".ynv"))
            {
                ynvList.Add(file);
            }
        }
    }

    public List<FilePickerFileType> GetSupportedFilesFilter()
    {
        List<FilePickerFileType> supportedFilesFilter = [];

        foreach (var format in SupportedFormats)
        {
            FilePickerFileType fpft = new(format.ToUpperInvariant().Replace("*.", "") + "(s) File(s)")
            {
                Patterns = [format]
            };
            supportedFilesFilter.Add(fpft);
        }

        supportedFilesFilter.Add(new FilePickerFileType("All GTA Assets")
        {
            Patterns = SupportedFormats
        });
        return supportedFilesFilter;
    }
}