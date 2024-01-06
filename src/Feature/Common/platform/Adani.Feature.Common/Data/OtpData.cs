namespace Adani.Feature.Common.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OtpData")]
    public partial class OtpData
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Key { get; set; }

        [Column(Order = 2)]
        [StringLength(10)]
        public string OTP { get; set; }

        [Column(Order = 3)]
        public DateTime ExpireAt { get; set; }
    }
}
