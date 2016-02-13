using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TSST.Agile.Models
{
    public class ProjectViewModel
    {
        private int id;
        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string title;

        [Required]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description;

        [Required]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTime startDate;

        [Required]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime endDate;

        [Required]
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private ICollection<int> userIdList;

        public ICollection<int> UserIdList
        {
            get { return userIdList; }
            set { userIdList = value; }
        }

        public ProjectViewModel()
        {
            UserIdList = new List<int>();
        }
    }
}
