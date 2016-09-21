using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueTime
{
    /// <summary>
    /// Holds the various events that are logged in the audit trail
    /// </summary>
    public enum AuditActivity
    {
        Login = 1,
        Logout
    }
    /// <summary>
    /// Models the audit trail of the system
    /// </summary>
    public class AuditTrail : EntityBase
    {
        public SystemUser ProjectUser { get; set; }
        public AuditActivity Activity { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }
}