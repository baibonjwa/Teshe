using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Teshe.Models
{
    interface IExportExcel
    {
        MemoryStream Export<T>(IList<T> data);  
    }
}
