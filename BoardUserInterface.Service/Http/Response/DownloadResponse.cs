using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardUserInterface.Service.Http.Response
{
    public class DownloadResponse
    {
        public string FileContentBase64 { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}