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
    
    public partial class Picture
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string Picture1 { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
