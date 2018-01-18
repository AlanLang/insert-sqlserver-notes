using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace insert_sqlserver_notes
{
    public class Table
    {
        public string name { get; set; }
        public string note { get; set; }

        public List<Column> column { get; set; }
    }

    public class Column
    {
        public string name { get; set; }
        public string note { get; set; }
    }

    public class ymldoc
    {
        public List<Table> table { get; set; }
    }
}
