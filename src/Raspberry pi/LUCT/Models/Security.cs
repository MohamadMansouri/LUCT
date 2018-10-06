using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models
{
    public class Security
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Password { get; set; }
        public int BanQuota { get; set; }
        public int BanTime { get; set; }
        public string KeyPadPassword { get; set; }
    }
}
