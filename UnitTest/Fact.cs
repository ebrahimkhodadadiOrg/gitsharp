        [Fact]
        public void Deliver_should_change_OrderState_to_deliver_when_order_state_is_shipped()
        {
            var order = new OrderBuilder().WithDefaultValues();
            order.Confirm();
            order.Process();
            order.Ship();

            order.Deliver();

            order.State.StateId.Should().Be(OrderStateType.Delivered.Id);
        }