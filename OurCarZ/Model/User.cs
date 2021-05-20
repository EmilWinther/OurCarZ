﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OurCarZ.Model
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            MessageMessageFromNavigations = new HashSet<Message>();
            MessageMessageToNavigations = new HashSet<Message>();
            Routes = new HashSet<Route>();
            UserRoutes = new HashSet<UserRoute>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(7)]
        public string LicensePlate { get; set; }
        public string Password { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }

        [ForeignKey(nameof(LicensePlate))]
        [InverseProperty(nameof(Car.Users))]
        public virtual Car LicensePlateNavigation { get; set; }
        [InverseProperty(nameof(Message.MessageFromNavigation))]
        public virtual ICollection<Message> MessageMessageFromNavigations { get; set; }
        [InverseProperty(nameof(Message.MessageToNavigation))]
        public virtual ICollection<Message> MessageMessageToNavigations { get; set; }
        [InverseProperty(nameof(Route.User))]
        public virtual ICollection<Route> Routes { get; set; }
        [InverseProperty(nameof(UserRoute.User))]
        public virtual ICollection<UserRoute> UserRoutes { get; set; }
    }
}