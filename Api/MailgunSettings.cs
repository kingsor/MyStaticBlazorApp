using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class MailgunSettings
    {
        public string MailgunApiKey { get; set; }
        public string NotifyEmails { get; set; }
        public string DomainName { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string SubjectDateFormat { get; set; }
    }
}
