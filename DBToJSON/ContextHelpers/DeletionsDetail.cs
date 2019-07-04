using DBToJSON.SqlEntities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBToJSON.ContextHelpers
{
    public class DeletionsDetail
    {
        public NosqlTable Table { get; set; }
        public IEnumerable<int> TableIds { get; set; }
    }
}
