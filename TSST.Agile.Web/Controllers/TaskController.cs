using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TSST.Agile.Database.Configuration.Interfaces;
using TSST.Agile.Database.Models;
using TSST.Agile.Models;

namespace TSST.Agile.Web.Controllers
{
    [Authorize]
    public class TaskController: IdentityApiController
    {
        private IGenericRepository<Task> _taskRepository;
        private IGenericRepository<Project> _projectRepository;
        private IGenericRepository<User> _userRepository;

        public TaskController(IGenericRepository<Task> taskRepository, IGenericRepository<Project> projectRepository, IGenericRepository<User> userRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ICollection<TaskViewModel>> GetProjectTasks(int projectId)
        {
            var tasks = await _taskRepository.FindByAsync(x => x.ProjectId == projectId);
            if (tasks != null)
            {
                var taskViewModelList = Mapper.Map<IEnumerable<Task>, ICollection<TaskViewModel>>(tasks);
                return taskViewModelList;

            }
            else
                return new List<TaskViewModel>();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task CreateTask(int projectId, TaskViewModel taskModel)
        {
            if(ModelState.IsValid && _id != -1)
            {
                var user = await _userRepository.EntityFindByAsync(x => x.Id == _id);
                var project = user?.Projects?.Where(x => x.Id == projectId).FirstOrDefault();
                if(user != null && project != null)
                {
                    var task = Mapper.Map<Task>(taskModel);
                    task.ProjectId = project.Id;
                    task.CreationDate = DateTime.Now;
                    task.State = TaskState.ToDo;
                    user.Tasks.Add(task);
                    _userRepository.Save();
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            var task = await _taskRepository.EntityFindByAsync(x => x.Id == taskId);
            if(task != null && task.UserId == _id)
            {
                _taskRepository.Delete(task);
                _taskRepository.Save();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task SetStateInProgress(int taskId)
        {
            var task = await _taskRepository.EntityFindByAsync(x => x.Id == taskId);
            if(task != null && task.UserId == _id)
            {
                task.State = TaskState.InProgress;
                task.StartDate = DateTime.Now;
                _taskRepository.Save();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task SetStateComplete(int taskId)
        {
            var task = await _taskRepository.EntityFindByAsync(x => x.Id == taskId);
            if (task != null && task.UserId == _id)
            {
                task.State = TaskState.Complete;
                task.CompleteDate = DateTime.Now;
                _taskRepository.Save();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}