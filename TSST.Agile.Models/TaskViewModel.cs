using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSST.Agile.Database.Models;

namespace TSST.Agile.Models
{
    public class TaskViewModel
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

        private DateTime creationDate;

        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        private DateTime completeDate;

        public DateTime CompleteDate
        {
            get { return completeDate; }
            set { completeDate = value; }
        }

        private TaskState state;

        public TaskState State
        {
            get { return state; }
            set { state = value; }
        }

        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private ICollection<FileViewModel> files;

        public ICollection<FileViewModel> Files
        {
            get { return files; }
            set { files = value; }
        }

        public TaskViewModel()
        {
            Files = new List<FileViewModel>();
        }
    }
}
