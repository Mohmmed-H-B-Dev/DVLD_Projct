using DVLD.Global__Classes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_With_MY_teatcher.Global__Classes.Messages_Service
{
    public class EmailService : MessageService
    {
        private bool _IsSendSuccessful = false;
        public bool IsSendSuccessful { get => _IsSendSuccessful;  }

        public void SendMessage(string receiver, string message)
        {
          Console.WriteLine($"Sending Email to {receiver}: {message}");

            //this is a simulation of sending email, in real application you would use an email sending library or API
            // Simulate email sending process  and if sending is successful, set _IsSendSuccessful to true
            //there you can add your logic to send email and set _IsSendSuccessful based on the result of sending email
            if (true)
            {
                _IsSendSuccessful = true;
            }
        }


        void RandomCodeGenerator(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string str = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        void MessageService.RandomCodeGenerator(int length)
        {
            RandomCodeGenerator(length);
        }

        string GeneratedCode { get; }

        string MessageService.GeneratedCode => GeneratedCode;
    }
}
