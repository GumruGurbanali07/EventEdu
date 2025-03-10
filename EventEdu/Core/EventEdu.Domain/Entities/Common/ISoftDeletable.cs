using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Domain.Entities.Common
{
    public interface ISoftDeletable
    {
        void SoftDelete();
        void Restore();

    }
}