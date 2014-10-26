using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("内容")]
        public string Contents { get; set; }

        [DisplayName("发送时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime SendTime { get; set; }

        [DisplayName("接收用户")]
        public UserInfo ReceivedUser { get; set; }

        [DisplayName("是否已读")]
        public int IsRead { get; set; }
    }
}