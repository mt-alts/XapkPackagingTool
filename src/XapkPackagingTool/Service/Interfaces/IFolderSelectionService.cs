using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XapkPackagingTool.Service.Interfaces
{
    internal interface IFolderSelectionService
    {
        (bool,string) OpenDialog(string title);
    }
}
