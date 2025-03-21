using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase03.Infra.Messages.Helpers
{
    public interface IMailHelper
    {
        void Send(string mailTo, string subject, string body);
    }
}

