using HelsiTaskManager.Repository.Dto;
using HelsiTaskManager.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using static HelsiTaskManager.WebAPI.Attributes.HelsiRigthCheckAttribute;

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

        /// <summary>
        /// Отримати один існуючий список задач
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [HelsiRigthCheck(HelsiRightType.Full)]
        public async Task<TaskList> Get([FromHeader(Name = "X-User-Id")] string xUserId, ObjectId id) 
            => await _taskListService.Get(id);

        /// <summary>
        /// Створити новий список задач
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task Add([FromBody] TaskList taskList) 
            => await _taskListService.Add(taskList);

        /// <summary>
        /// Змінити існуючий список задач
        /// </summary>
        /// <param name="taskList"></param>
        /// <returns></returns>
        [HttpPut]
        [HelsiRigthCheck(HelsiRightType.Full)]
        public async Task<BaseResponse> Udpate(TaskListRequest taskList)
            => await _taskListService.Udpate(taskList);

        [HttpGet]
        [HelsiRigthCheck(HelsiRightType.Full)]
        public async Task<TaskListsResponse> GetAll([FromQuery]ObjectId userId) 
            => await _taskListService.GetAll(userId);

        /// <summary>
        ///  Отримати список списків задач
        /// </summary>
        /// <param name="id"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("{id}&{userId}&{skip}&{limit}")]
        [HelsiRigthCheck(HelsiRightType.Full)]
        public async Task<GetGoalsInTaskResponse> GetGoalsInTask(ObjectId id, ObjectId userId, int skip, int limit)
            => await _taskListService.GetGoalsInTask(id,skip, limit);

        /// <summary>
        /// Додати зв’язок одного списку задач з вказаним користувачем
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [HelsiRigthCheck(HelsiRightType.Full)]
        public async Task<BaseResponse> AddLinkedUser([FromBody]AddLinkedUserRequest request) 
            => await _taskListService.AddLinkedUser(request);

        /// <summary>
        /// Прибрати зв’язок одногоодного списку задач з вказаним користувачем
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [HelsiRigthCheck(HelsiRightType.Full)]
        public async Task<BaseResponse> RemoveLinkedUser([FromBody]AddLinkedUserRequest request) 
            => await _taskListService.RemoveLinkedUser(request);

        /// <summary>
        /// Отримати зв’язки одного списку задач з користувачами
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("userId")]
        public async Task<TaskListsResponse> GetTaskListByLinkedUser(ObjectId userId) 
            => await _taskListService.GetTaskListByLinkedUser(userId);

        /// <summary>
        /// Видалити існуючий список задач
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}&{userId}")]
        [HelsiRigthCheck(HelsiRightType.Owner)]
        public async Task<BaseResponse> Remove(ObjectId id, ObjectId userId) 
            => await _taskListService.Remove(id);
    }
}