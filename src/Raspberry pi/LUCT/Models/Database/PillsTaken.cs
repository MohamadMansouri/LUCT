﻿using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models.Database
{
    public class PillsTaken
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public DateTime Time { get; set; }
    }
}
