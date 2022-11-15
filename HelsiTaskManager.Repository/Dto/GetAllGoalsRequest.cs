using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelsiTaskManager.Repository.Dto
{
    public class GetAllGoalsRequest: BaseRequest
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
