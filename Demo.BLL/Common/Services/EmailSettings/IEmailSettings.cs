using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Common.Services.EmailSettings
{
    public interface IEmailSettings
    {
        public void SendEmail(DAL.Entities.Identity.Email email);
    }
}
