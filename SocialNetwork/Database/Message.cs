//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocialNetwork.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int ID { get; set; }
        public string FromID { get; set; }
        public string ToID { get; set; }
        public string Message1 { get; set; }
        public int MessageType { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string BeenRead { get; set; }
    
        public virtual MessageType MessageType1 { get; set; }
        public virtual User User { get; set; }
    }
}
