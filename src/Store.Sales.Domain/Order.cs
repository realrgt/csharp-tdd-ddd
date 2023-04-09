using FluentValidation.Results;
using Store.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Sales.Domain
{
    public class Order
    {
        public static int MAX_ITEM_UNITS => 15;
        public static int MIN_ITEM_UNITS => 1;


        public Guid CustomerId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool HasUsedVoucher { get; private set; }
        public Voucher Voucher { get; private set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public OrderStatus OrderStatus { get; private set; }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        private void ComputeOrderPrice()
        {
            TotalAmount = OrderItems.Sum(item => item.ComputePrice());
        }

        private bool IsExistingOrderItem(OrderItem item)
        {
            return _orderItems.Any(p => p.ProductId == item.ProductId);
        }

        private void ValidateUnexistingOrderItem(OrderItem item)
        {
            if (!IsExistingOrderItem(item)) throw new DomainException(message: $"Item not present into the order.");
        }

        private void ValidateAllowedItemQuantity (OrderItem item)
        {
            var itemsQuantity = item.Quantity;
            if (IsExistingOrderItem(item))
            {
                var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == item.ProductId);
                itemsQuantity += existingItem.Quantity;
            }

            if (itemsQuantity > MAX_ITEM_UNITS) throw new DomainException(message: $"Maximmum of {MAX_ITEM_UNITS} units.");
        }

        public void AddItem(OrderItem orderItem)
        {
            ValidateAllowedItemQuantity(orderItem);

            if (IsExistingOrderItem(orderItem))
            {
                var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

                existingItem.AddUnits(orderItem.Quantity);
                orderItem = existingItem;
                _orderItems.Remove(existingItem);
            }

            _orderItems.Add(orderItem);
            ComputeOrderPrice();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            ValidateUnexistingOrderItem(orderItem);
            ValidateAllowedItemQuantity(orderItem);

            var existingitem = OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

            _orderItems.Remove(existingitem);
            _orderItems.Add(orderItem);

            ComputeOrderPrice();
        }

        public void RemoveItem(OrderItem orderItem)
        {
            ValidateUnexistingOrderItem(orderItem);

            _orderItems.Remove(orderItem);
            
            ComputeOrderPrice();
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var result = voucher.ValidateIsApplicable();
            if (!result.IsValid) return result;

            Voucher = voucher;
            HasUsedVoucher = true;

            return result;
        }

        public void MakeDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public static class OrderFactory
        {
            public static Order NewDraftOrder(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                };

                order.MakeDraft();
                return order;
            }
        }
    }
}
