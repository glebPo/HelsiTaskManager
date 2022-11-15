using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelsiTaskManager.Repository
{
    public class Goal: BaseEntity
    {
        public ObjectId Id { get; set; }
        public string Description { get; set; }
    }
}
