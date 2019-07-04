using DBToJSON.SqlEntities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DBToJSON.SqlEntities
{
    public class RecordDeletionTime
    {
        [Key]
        [Column(Order = 1)]
        public NosqlTable TableId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int IdOfDeletedRecord { get; set; }
        private DateTime _deleted;
        public DateTime Deleted { get => _deleted; internal set => _deleted = value.AsUtc(); }
    }
}
