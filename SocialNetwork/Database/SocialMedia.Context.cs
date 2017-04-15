﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SocialMediaEntities : DbContext
    {
        public SocialMediaEntities()
            : base("name=SocialMediaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<FriendsStatu> FriendsStatus { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<PostComment> PostComments { get; set; }
        public virtual DbSet<PostRep> PostReps { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<string> SearchUsers(string input)
        {
            var inputParameter = input != null ?
                new ObjectParameter("input", input) :
                new ObjectParameter("input", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SearchUsers", inputParameter);
        }
    
        public virtual ObjectResult<SearchForUsers_Result> SearchForUsers(string input)
        {
            var inputParameter = input != null ?
                new ObjectParameter("input", input) :
                new ObjectParameter("input", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SearchForUsers_Result>("SearchForUsers", inputParameter);
        }
    
        public virtual ObjectResult<SearchForUsersNotMe_Result> SearchForUsersNotMe(string input, string inputID)
        {
            var inputParameter = input != null ?
                new ObjectParameter("input", input) :
                new ObjectParameter("input", typeof(string));
    
            var inputIDParameter = inputID != null ?
                new ObjectParameter("inputID", inputID) :
                new ObjectParameter("inputID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SearchForUsersNotMe_Result>("SearchForUsersNotMe", inputParameter, inputIDParameter);
        }
    }
}