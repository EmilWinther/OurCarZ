﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OurCarZ.Model
{
    [Table("Car")]
    public partial class Car
    {
        public Car()
        {
            Users = new HashSet<User>();
        }

        [StringLength(50)]
        public string Model { get; set; }
        [StringLength(50)]
        public string Year { get; set; }
        public int? Seats { get; set; }
        [Key]
        [StringLength(7)]
        public string LicensePlate { get; set; }

        [InverseProperty(nameof(User.LicensePlateNavigation))]
        public virtual ICollection<User> Users { get; set; }
    }
}