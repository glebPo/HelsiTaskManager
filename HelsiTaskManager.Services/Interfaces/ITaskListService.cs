using HelsiTaskManager.Repository.Dto;

namespace HelsiTaskManager.Services;
public interface ITaskListService
{
    Task<TaskList> Get(ObjectId request);
    Task<BaseResponse> Add(TaskList taskList);
    Task<BaseResponse> Udpate(TaskList taskList);
    Task<BaseResponse> Remove(ObjectId id);
    Task<TaskListsResponse> GetAll(ObjectId userId);
    Task<GetGoalsInTaskResponse> GetGoalsInTask(ObjectId userId, int skip, int limit);
    Task<BaseResponse> RemoveLinkedUser(AddLinkedUserRequest request);
    Task<BaseResponse> AddLinkedUser(AddLinkedUserRequest request);
    Task<TaskListsResponse> GetTaskListByLinkedUser(ObjectId userId);
}
