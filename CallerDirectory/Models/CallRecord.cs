using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallerDirectory.Models
{
    [Index(nameof(Caller))]
    [Index(nameof(Recipient))]
    [Index(nameof(StartDateTime))]
    [Index(nameof(EndDateTime))]
    [Index(nameof(Reference), IsUnique = true)]
    public class CallRecord
    {
        public int Id { get; set; }

        public long? Caller { get; set; }

        public long Recipient { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public float Cost { get; set; }

        [Column(TypeName = "VARCHAR(33)")]
        public string Reference { get; set; }

        [Column(TypeName = "VARCHAR(3)")]
        public string Currency { get; set; }
    }
}
