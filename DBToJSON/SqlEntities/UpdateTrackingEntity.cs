using Newtonsoft.Json;
using System;

namespace DBToJSON.SqlEntities
{
    public abstract class UpdateTrackingEntity
    {
        private DateTime _dateModified;
        [JsonIgnore]
        public DateTime DateModified
        {
            get => _dateModified;
            internal set => _dateModified = value.AsUtc();
        }
    }
    internal interface INosqlTable
    {
    }
}