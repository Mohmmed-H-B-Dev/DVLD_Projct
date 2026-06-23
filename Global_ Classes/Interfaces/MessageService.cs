using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Global__Classes.Interfaces
{
    public   interface MessageService
    {
        void SendMessage(string receiver, string message);
        bool IsSendSuccessful { get;  }

        void RandomCodeGenerator(int length);
        string GeneratedCode { get; }

    }
}
