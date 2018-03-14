using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _2th.Entities
{
    [Table("Command")]
    public class Command
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(1024)]
        public string Content { get; set; }
        public string Auther { get; set; }
        [Required]
        [MaxLength(255)]
        public string TagName { get; set; }
        public string Img { get; set; }


    }
}