using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models
{
    public class LeftMenuItem
    {
        public string Label { get; set; }

        public List<LeftMenuItem> GetSampleItems(string type)
        {
            return new List<LeftMenuItem>()
            {
                new LeftMenuItem()
                {
                    Label = "About"
                },
                new LeftMenuItem()
                {
                    Label = "Group"
                },
                new LeftMenuItem()
                {
                    Label = "People"
                },
                new LeftMenuItem()
                {
                    Label = "Some"
                }
            };
        }
    }
}
