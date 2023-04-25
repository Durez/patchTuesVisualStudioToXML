using Microsoft.EntityFrameworkCore;
using patchTuesVisualStudioToXML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML.Controllers.DBControllers
{
    internal class OVALXMLDBController
    {
        public async Task<OVALXMLs> GetById(long id)
        {
            using (var db = new OVALXMLsContext())
            {
                return await db.OVALXMLs.FindAsync(id);
            }
        }

        public async Task<List<OVALXMLs>> GetByDateTime(DateTime dateTime)
        {
            using (var db = new OVALXMLsContext())
            {
                return await db.OVALXMLs.Where(t => t.CreationDate == dateTime).ToListAsync();
            }
        }

        public async Task Insert(OVALXMLs ovalXML)
        {
            using var db = new OVALXMLsContext();
            await db.OVALXMLs.AddAsync(ovalXML);
            await db.SaveChangesAsync();
        }
    }
}
