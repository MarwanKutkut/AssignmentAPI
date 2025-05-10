namespace AssignmentAPI.StartupExtensions.Seeders
{
    public class SeederAttribute : Attribute
    {
        public int OrderId { get; set; }
        public SeederAttribute(int OrderId = Order.Default)
        {
            this.OrderId = OrderId;
        }
    }

    public static class Order
    {
        public const int Default = 1000;
        public const int First = 1;
        public const int Second = 2;
        public const int Third = 3;
    }
}
