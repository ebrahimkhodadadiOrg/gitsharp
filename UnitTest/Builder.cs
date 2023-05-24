    public class OrderBuilder
    {
        private Order _order;

        public int? TestUserId => null;
        public List<OrderItem> TestOrderItems => new() { new OrderItemBuilder().WithDefaultValues() };

        public OrderBuilder()
        {
            _order = WithDefaultValues();
        }

        public Order Build()
        {
            return _order;
        }

        public Order WithDefaultValues()
        {
            return new Order(TestUserId, TestOrderItems);
        }
    }