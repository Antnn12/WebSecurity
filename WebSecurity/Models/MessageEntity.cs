using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSecurity_Anton.Models
{
    public class MessageEntity
    {
        [Key]
        public int Id { get; set; }

        public string Message { get; set; }

        public string Name { get; set; }
    }
}
