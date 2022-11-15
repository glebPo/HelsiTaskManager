using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelsiTaskManager.Repository;

public class TaskListResponse: BaseResponse
{
    public TaskList TaskList { get; set; }
}
