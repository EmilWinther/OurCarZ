﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OurCarZ.Model
{
    [Table("Institution")]
    public partial class Institution
    {
        [Key]
        [Column("InstitutionID")]
        public int InstitutionId { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(50)]
        public string Zipcode { get; set; }
    }
}