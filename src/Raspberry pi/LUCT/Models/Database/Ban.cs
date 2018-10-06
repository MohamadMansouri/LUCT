using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models.Database
{
    public class Ban
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Image { get; set; } = "nothing ...";
        public DateTime Time { get; set; }
    }
}
