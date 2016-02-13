﻿using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TSST.Agile.Database.Configuration.Interfaces;
using TSST.Agile.Database.Models;
using TSST.Agile.Models;
using TSST.Agile.Web.Helpers;

namespace TSST.Agile.Web.Controllers
{
    [Authorize]
    public class ProjectController: IdentityApiController
    {
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Project> _projectRepository;

        public ProjectController(IGenericRepository<User> userRepository, IGenericRepository<Project> projectRepository)
        {
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public ICollection<ProjectViewModel> GetMyProjects()
        {
            int userId = 0;
            if (Int32.TryParse(_id, out userId))
            {
                var projects = _projectRepository.ExecWithStoreProcedure("exec GetUserProjects @UserId", new SqlParameter("UserId", userId));
                var projectViewModelList = Mapper.Map<IEnumerable<Project>, ICollection<ProjectViewModel>>(projects);
                return projectViewModelList;
            }
            else
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task CreateProject(ProjectViewModel projectModel)
        {
            if (ModelState.IsValid && projectModel?.UserIdList.Count > 0)
            {
                var project = Mapper.Map<Project>(projectModel);
                var users = await _userRepository.FindByAsync(x => projectModel.UserIdList.Contains(x.Id));
                if(users != null)
                {
                    project.Users = users.ToList();
                    foreach(var user in users)
                    {
                        user.Projects.Add(project);
                    }
                    _userRepository.Save();
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        public async System.Threading.Tasks.Task DeleteProject(int id)
        {
            var project = await _projectRepository.EntityFindByAsync(x => x.Id == id);
            if (project != null)
            {
                _projectRepository.Delete(project);
                _projectRepository.Save();
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}