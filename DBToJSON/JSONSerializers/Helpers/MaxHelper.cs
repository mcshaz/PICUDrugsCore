﻿using DBToJSON.SqlEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBToJSON.JSONSerializers.Helpers
{
    static class MaxHelper
    {
        public static DateTime GetMax(UpdateTrackingEntity entity, params IEnumerable<UpdateTrackingEntity>[] collections)
        {
            return (from c in collections
                    where c.Any()
                    select c.Max(s => s.DateModified)).Concat(new[] { entity.DateModified }).Max();
        }
    }
}
