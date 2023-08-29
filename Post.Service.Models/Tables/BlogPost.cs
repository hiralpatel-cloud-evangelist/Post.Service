using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Post.Service.Models.Tables;

public partial class BlogPost
{
    [Key]
    [Column("BlogPostID")]
    public long BlogPostId { get; set; }

    [Column("BlogPostSID")]
    [StringLength(50)]
    [Unicode(false)]
    public string BlogPostSid { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? PostName { get; set; }

    public string? PostDescription { get; set; }

    [Column("AccountID")]
    public long? AccountId { get; set; }

    public byte? Status { get; set; }

    public string? BlogImage { get; set; }

    [Column("CreatedByUserID")]
    public long? CreatedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDatetime { get; set; }

    [Column("LastModifiedByUserID")]
    public long? LastModifiedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastModifiedDatetime { get; set; }
}
