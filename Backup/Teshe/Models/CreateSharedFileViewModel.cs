using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class CreateSharedFileViewModel
    {
        public string Id { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("文件描述")]
        public string Description { get; set; }

        [DisplayName("文件名")]
        public string OldFileName { get; set; }

        [DisplayName("新文件名")]
        public string NewFileName { get; set; }

    }
}   