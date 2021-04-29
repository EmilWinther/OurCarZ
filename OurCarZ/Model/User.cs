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
        [Column("CarID")]
        public int? CarId { get; set; }
        public double? Rating { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        [ForeignKey(nameof(CarId))]
        [InverseProperty("Users")]
        public virtual Car Car { get; set; }
        [InverseProperty(nameof(Route.User))]
        public virtual ICollection<Route> Routes { get; set; }
        [InverseProperty(nameof(UserRoute.User))]
        public virtual ICollection<UserRoute> UserRoutes { get; set; }
    }
}