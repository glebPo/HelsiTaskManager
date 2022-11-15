using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelsiTaskManager.Repository.Dto
{
    public class TaskListRequest: BaseRequest
    {
        public TaskList TaskList { get; set; }
    }
}
