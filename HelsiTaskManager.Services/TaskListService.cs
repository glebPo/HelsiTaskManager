using HelsiTaskManager.Repository;
using HelsiTaskManager.Repository.Dto;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Diagnostics;

namespace HelsiTaskManager.Services;
public class TaskListService : ITaskListService
{
    private readonly IUnitOfWork _unitOfWork;
    public TaskListService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    /// <summary>
    /// Отримати один існуючий список задач
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TaskList> Get(ObjectId id) => await _unitOfWork.TaskList.GetAsync(id);

    /// <summary>
    /// Створити новий список задач
    /// </summary>
    /// <param name="taskList"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse> Add(TaskList taskList)
    {
        var response = new BaseResponse();
        if (!await _unitOfWork.User.AnyAsync(x => x.Id == taskList.OwnerId))
        {
            throw new Exception("Invalid UserId");
        }
        var result = await _unitOfWork.TaskList.AddAsync(taskList);
        response.IsSuccess = result > 0;
        return response;
    }
    /// <summary>
    /// Змінити існуючий список задач
    /// </summary>
    /// <param name="taskList"></param>
    /// <returns></returns>
    public async Task<BaseResponse> Udpate(TaskListRequest request)
    {
        var response = new BaseResponse();
        var result = await _unitOfWork.TaskList.Update(request.TaskList);
        response.IsSuccess = result > 0;
        return response;
    }
    /// <summary>
    /// Видалити існуючий список задач
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<BaseResponse> Remove(TaskListRequest request)
    {
        var response = new BaseResponse();
        var result = await _unitOfWork.TaskList.RemoveAsync(request.Id);
        response.IsSuccess = result > 0;
        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<TaskListsResponse> GetAll(ObjectId userId)
    {
        var response = new TaskListsResponse { IsSuccess = true };
        var collection = _unitOfWork.TaskList.Context.GetCollection<TaskList>("TaskList").AsQueryable();
        response.List = collection.Where(x => x.OwnerId == userId).ToList();
        return response;
    }
    /// <summary>
    ///  Отримати список списків задач
    /// </summary>
    /// <param name="id"></param>
    /// <param name="skip"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<GetGoalsInTaskResponse> GetGoalsInTask(ObjectId id, int skip, int limit)
    {
        //TODO: change on DB aggregation?

        var response = new GetGoalsInTaskResponse { IsSuccess = true };
        var collection = _unitOfWork.TaskList.Context.GetCollection<TaskList>("TaskList");
        var filter = Builders<TaskList>.Filter.Eq(x => x.Id, id);
        var taskList = await collection.FindAsync(filter);
        var goals = taskList.FirstOrDefault().Goals.OrderBy(x => x.Created).Skip(skip).Take(limit);
        response.Goals = goals.ToList();
        return response;
    }
    /// <summary>
    /// Додати зв’язок одного списку задач з вказаним користувачем
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<BaseResponse> AddLinkedUser(AddLinkedUserRequest request)
    {
        var response = new BaseResponse();
        var taskList = await _unitOfWork.TaskList.GetAsync(request.Id);
        taskList.LinkedUsers.Add(request.LinkedUserId);
        var result = await _unitOfWork.TaskList.Update(taskList);
        response.IsSuccess = result > 0;
        return response;
    }
    /// <summary>
    /// Прибрати зв’язок одногоодного списку задач з вказаним користувачем
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<BaseResponse> RemoveLinkedUser(AddLinkedUserRequest request)
    {
        var result = 0;
        var response = new BaseResponse { IsSuccess = true };
        var taskList = await _unitOfWork.TaskList.GetAsync(request.Id);
        if (taskList.LinkedUsers.Remove(request.LinkedUserId)) {
            await _unitOfWork.TaskList.Update(taskList);
        }
        response.IsSuccess = result > 0;
        return response;
    }
    /// <summary>
    /// Отримати зв’язки одного списку задач з користувачами
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<TaskListsResponse> GetTaskListByLinkedUser(ObjectId userId)
    {
        var response = new TaskListsResponse { IsSuccess = true };
        var collection = _unitOfWork.TaskList.Context.GetCollection<BsonDocument>("TaskList");
        var filter = Builders<BsonDocument>.Filter.All("LinkedUsers", new[] { userId });
        var bsonTasks = await collection.FindAsync<BsonDocument>(filter);
        response.List = bsonTasks.ToList().Select(v => BsonSerializer.Deserialize<TaskList>(v)).ToList();
        return response;
    }
    /// <summary>
    /// Видалити існуючий список задач
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<BaseResponse> Remove(ObjectId id)
    {
        var response = new BaseResponse { IsSuccess = true };
        response.IsSuccess = await _unitOfWork.TaskList.RemoveAsync(id) > 0;
        return response;
    }
}