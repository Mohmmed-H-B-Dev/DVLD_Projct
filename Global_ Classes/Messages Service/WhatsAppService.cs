using DVLD.Global__Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_With_MY_teatcher.Global__Classes.Messages_Service
{
    internal class WhatsAppService : MessageService
    {
        private bool _IsSendSuccessful = false;
        public bool IsSendSuccessful { get => _IsSendSuccessful; }

        public string GeneratedCode => throw new NotImplementedException();

        public void RandomCodeGenerator(int length)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string receiver, string message)
        {
           Console.WriteLine($"Sending WhatsApp message to {receiver}: {message}");


            //this is a simulation of sending WhatsApp, in real application you would use an WhatsApp sending library or API
            // Simulate WhatsApp sending process  and if sending is successful, set _IsSendSuccessful to true
            //there you can add your logic to send WhatsApp and set _IsSendSuccessful based on the result of sending email
            if (true)
            {
                _IsSendSuccessful = true;
            }
        }
    }
}
