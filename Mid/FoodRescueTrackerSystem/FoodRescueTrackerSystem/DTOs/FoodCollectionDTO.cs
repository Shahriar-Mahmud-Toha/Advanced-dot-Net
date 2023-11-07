using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodRescueTrackerSystem.DTOs
{
    public class FoodCollectionDTO
    {
        public int Id { get; set; }
        public System.DateTime RequestTime { get; set; }
        public string Address { get; set; }
        public System.DateTime DueTime { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> ApprovalTime { get; set; }
        public Nullable<System.DateTime> CollectionTime { get; set; }
        public Nullable<System.DateTime> DistributionTime { get; set; }
        public string RequestCreatorEmail { get; set; }
        public string ApproverEmail { get; set; }
        public string DistributorEmail { get; set; }
    }
}