using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoApp.Entity
{
    public class PageModel
    {
        public int Current { get; set; }
        public int RowCount { get; set; }
        public string Sort { get; set; }
        public string SearchPhrase { get; set; }
    }
}
