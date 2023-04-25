using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML.DAL.Entities
{

    [Table("OVALXMLs")]
    public class OVALXML
    {
        //[Column("id", TypeName = "bigserial")]
        public long Id { get; set; }

        [Column("data", TypeName = "bytea")]
        public byte[] Data { get; set; }

        [Column("creation_date", TypeName = "timestamp")]
        public DateTime CreationDate { get; set;}

    }

}
