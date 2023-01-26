using System;

namespace ToDoApp.Contract
{
    [Serializable]
    public class ContractBase
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
