﻿using Microsoft.AspNetCore.Http;
using RockwellBugTracker.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static RockwellBugTracker.Extensions.MaxFileSizeAttribute;

namespace RockwellBugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        [DisplayName("File Description")]
        public string Description { get; set; }
        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; }
        [DisplayName("Ticket")]
        public int TicketId { get; set; }
        [DisplayName("Team Member")]
        public int UserId { get; set; }
        [Display(Name = "Select Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        [MaxFileSize(2 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" })]
        public IFormFile FormFile { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }

        [DisplayName("File Extension")]
        public string FileContentType { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual BTUser User { get; set; }

    }
}
