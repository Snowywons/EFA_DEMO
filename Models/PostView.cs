using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EFA_DEMO.Models
{
    public class PostView
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obligatoire")]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Obligatoire")]
        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Display(Name = "Mots-Clés")]
        public string Tags { get; set; }

        [Display(Name = "Création")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Obligatoire")]
        [Display(Name = "Utilisateur")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "Utilisateur")]
        public virtual UserView User { get; set; }

        public PostView()
        {
            CreationDate = DateTime.Now;
        }

        public Post ToPost()
        {
            return new Post()
            {
                Id = this.Id,
                Title = this.Title,
                Content = this.Content,
                Tags = this.Tags,
                CreationDate = this.CreationDate,
                UserId = this.UserId
            };
        }

        public void CopyToPost(Post post)
        {
            post.Id = Id;
            post.Title = Title;
            post.Content = Content;
            post.Tags = Tags;
            post.CreationDate = CreationDate;
            post.UserId = UserId;
        }
    }
}