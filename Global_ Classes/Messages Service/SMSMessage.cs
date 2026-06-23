using DVLD.Global__Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Project.Global__Classes.Messages_Service
{
    public  class SMSMessage : MessageService
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
            Console.WriteLine($"Sending SMS to {receiver}: {message}");


            //this is a simulation of sending SMS, in real application you would use an SMS sending library or API
            // Simulate SMS sending process  and if sending is successful, set _IsSendSuccessful to true
            //there you can add your logic to send SMS and set _IsSendSuccessful based on the result of sending email
            if (true)
            {
                _IsSendSuccessful = true;
            }
        }
    }
}
