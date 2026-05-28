using System;

namespace ShoeStore.Models
{
    public class OrderInfo
    {
        public int OrderId;
        public string OrderCode;
        public int StatusId;
        public string StatusName;
        public int PickupPointId;
        public string PickupAddress;
        public DateTime OrderDate;
        public DateTime? DeliveryDate;
    }
}
