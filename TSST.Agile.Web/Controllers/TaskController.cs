using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using TSST.Agile.Database.Configuration.Interfaces;
using TSST.Agile.Database.Models;
using TSST.Agile.FileStorage.Interfaces;
using TSST.Agile.Models;

namespace TSST.Agile.Web.Controllers
{
    [Authorize]
    public class TaskController: IdentityApiController
    {
        private IGenericRepository<Task> _taskRepository;
        private IGenericRepository<Project> _projectRepository;
        private IGenericRepository<User> _userRepository;
        private IFileStorage _fileStorage;

        public TaskController(IGenericRepository<Task> taskRepository, IGenericRepository<Project> projectRepository, IGenericRepository<User> userRepository, IFileStorage fileStorage)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _fileStorage = fileStorage;
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

        [HttpPut]
        public async System.Threading.Tasks.Task<HttpResponseMessage> AddFile()
        {
            var headers = Request.Headers;
            var taskId = headers.GetValues("TaskId").FirstOrDefault();
            var fileModel = new FileViewModel();
            fileModel.Name = headers.GetValues("FileName").FirstOrDefault();
            fileModel.Description = headers.GetValues("FileDescription").FirstOrDefault();
            var task = await _taskRepository.EntityFindByAsync(x => x.Id.ToString() == taskId);
            if (task != null)
            {
                var fileContent = await this.Request.Content.ReadAsStreamAsync();
                MediaTypeHeaderValue contentTypeHeader = this.Request.Content.Headers.ContentType;
                string contentType = contentTypeHeader != null ? contentTypeHeader.MediaType : "application/octet-stream";
                var ms = new MemoryStream();
                await fileContent.CopyToAsync(ms);
                fileContent.Close();
                string url = await _fileStorage.AddFile(task.ProjectId, task.Id, fileModel.Name, ms);
                task.Files.Add(new Database.Models.File() { Name = fileModel.Name, Description = fileModel.Description, FileUrl = url });
                _taskRepository.Save(); 
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            else
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}