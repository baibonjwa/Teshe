using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Teshe.Models
{
    public class Attribute
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("属性名称")]
        public string Name { get; set; }

        [DisplayName("属性内容")]
        public string Content { get; set; }

        [DisplayName("设备")]
        [Required]
        [JsonIgnore]
        public virtual Device Device { get; set; }
    }
}