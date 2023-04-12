using FluentValidation;
using Store.Core.Messages;
using Store.Sales.Domain;
using System;

namespace Store.Sales.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public AddOrderItemCommand(Guid clientId, Guid productId, string name, int quantity, decimal unitPrice)
        {
            ClientId = clientId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public static string IdClientErrorMsg => "Invalid client Id";
        public static string IdProductErrorMsg => "Invalid product Id";
        public static string NameErrorMsg => "Product name was not entered";
        public static string QuantityMaxErrorMsg => $"Item's maximum quantity is {Order.MAX_ITEM_UNITS}";
        public static string QuantityMinErrorMsg => "Item's minimum quantity is  1";
        public static string PriceErrorMsg => "Item's price should be greather than 0";

        public AddOrderItemValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdClientErrorMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(IdProductErrorMsg);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameErrorMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityMinErrorMsg)
                .LessThanOrEqualTo(Order.MAX_ITEM_UNITS)
                .WithMessage(QuantityMaxErrorMsg);

            RuleFor(c => c.UnitPrice)
                .GreaterThan(0)
                .WithMessage(PriceErrorMsg);
        }
    }
}
