using HelsiTaskManager.Repository.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HelsiTaskManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TaskListController: ControllerBase
    {
        private readonly ITaskListService _taskListService;
        public TaskListController(ITaskListService taskListService)
        {
            _taskListService = taskListService;
        }

        [HttpGet]
        public async Task<TaskList> Get([FromQuery]ObjectId id) 
            => await _taskListService.Get(id);

        [HttpPost]
        public async Task Add([FromBody] TaskList taskList) 
            => await _taskListService.Add(taskList);

        [HttpPut]
        public async Task<BaseResponse> Udpate(TaskList taskList)
            => await _taskListService.Udpate(taskList);

        [HttpGet]
        public async Task<TaskListsResponse> GetAll([FromQuery]ObjectId userId) 
            => await _taskListService.GetAll(userId);

        [HttpGet("{id}&{skip}&{limit}")]
        public async Task<GetGoalsInTaskResponse> GetGoalsInTask(ObjectId id, int skip, int limit)
            => await _taskListService.GetGoalsInTask(id, skip, limit);

        [HttpPut]
        public async Task<BaseResponse> AddLinkedUser([FromBody]AddLinkedUserRequest request) 
            => await _taskListService.AddLinkedUser(request);

        [HttpPut]
        public async Task<BaseResponse> RemoveLinkedUser([FromBody]AddLinkedUserRequest request) 
            => await _taskListService.RemoveLinkedUser(request);

        [HttpGet("userId")]
        public async Task<TaskListsResponse> GetTaskListByLinkedUser(ObjectId userId) 
            => await _taskListService.GetTaskListByLinkedUser(userId);

        [HttpDelete("{id}")]
        public async Task<BaseResponse> Remove([FromQuery]ObjectId id) 
            => await _taskListService.Remove(id);
    }
}