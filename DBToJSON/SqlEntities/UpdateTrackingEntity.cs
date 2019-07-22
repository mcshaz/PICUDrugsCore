using Newtonsoft.Json;
using System;

namespace DBToJSON.SqlEntities
{
    public abstract class INosqlTable
    {
        private DateTime _dateModified;
        [JsonIgnore]
        public DateTime DateModified
        {
            get => _dateModified;
            internal set => _dateModified = value.AsUtc();
        }
    }
}