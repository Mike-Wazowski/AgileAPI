using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST.Agile.Database.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FacebookId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Friendship> Friendships { get; set; }

        public User()
        {
            Projects = new List<Project>();
            Friendships = new List<Friendship>();
        }
    }
}
