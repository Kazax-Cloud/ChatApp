using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.MVVM.Model
{
    class MessageContact
    {
        public string Username { get; set; }

        public string UsernameCololor { get; set; }

        public string ImageSource { get; set; }

        public string Message { get; set; }

        public DateTime TimeWrite { get; set; }

        public bool IsNativeOrigin { get; set; }

        public bool FirstMessage { get; set; }
    }
}
