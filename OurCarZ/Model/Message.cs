﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OurCarZ.Model
{
    public partial class Message
    {
        [Key]
        [Column("MessageID")]
        public int MessageId { get; set; }
        public int? MessageTo { get; set; }
        [Required]
        public string MessageText { get; set; }
        public int? MessageFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateTime { get; set; }

        [ForeignKey(nameof(MessageFrom))]
        [InverseProperty(nameof(User.MessageMessageFromNavigations))]
        public virtual User MessageFromNavigation { get; set; }
        [ForeignKey(nameof(MessageTo))]
        [InverseProperty(nameof(User.MessageMessageToNavigations))]
        public virtual User MessageToNavigation { get; set; }
    }
}