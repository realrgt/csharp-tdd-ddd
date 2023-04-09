using FluentValidation;
using FluentValidation.Results;
using System;

namespace Store.Sales.Domain
{
    public class Voucher 
    {
        public string Code { get; private set; }
        public decimal? DiscountAmount { get; private set; }
        public decimal? DiscountPercent { get; private set; }
        public DiscountType DiscountType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime Validity { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsUsed { get; private set; }

        public Voucher(string code, decimal? discountAmount, decimal? discountPercent, DiscountType discountType, int quantity, DateTime validity, bool isActive, bool isUsed)
        {
            Code = code;
            DiscountAmount = discountAmount;
            DiscountPercent = discountPercent;
            DiscountType = discountType;
            Quantity = quantity;
            Validity = validity;
            IsActive = isActive;
            IsUsed = isUsed;
        }

        public ValidationResult ValidateIsApplicable()
        {
            return new VoucherApplicableValidation().Validate(this);
        }
    }

    public class VoucherApplicableValidation : AbstractValidator<Voucher>
    {
        public static string CodeErrorMsg => "Invalid Voucher Code";
        public static string ValidityErrorMsg => "Expired Voucher";
        public static string IsActiveErrorMsg => "No longer valid Voucher";
        public static string IsUsedErrorMsg => "Used Voucher";
        public static string QuantityErrorMsg => "No longer availabley Voucher";
        public static string DiscountAmountErrorMsg => "Discount amount must be greater than 0";
        public static string DiscountPercentErrorMsg => "Discount percent must be greater than 0";

        public VoucherApplicableValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(CodeErrorMsg);

            RuleFor(c => c.Validity)
                .Must(ValidityGreaterThanNow)
                .WithMessage(ValidityErrorMsg);

            RuleFor(c => c.IsActive)
                .Equal(true)
                .WithMessage(IsActiveErrorMsg);

            RuleFor(c => c.IsUsed)
                .Equal(false)
                .WithMessage(IsUsedErrorMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityErrorMsg);

            When(f => f.DiscountType == DiscountType.Amount, () =>
            {
                RuleFor(f => f.DiscountAmount)
                    .NotNull()
                    .WithMessage(DiscountAmountErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(DiscountAmountErrorMsg);
            });

            When(f => f.DiscountType == DiscountType.Percentage, () =>
            {
                RuleFor(f => f.DiscountPercent)
                    .NotNull()
                    .WithMessage(DiscountPercentErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(DiscountPercentErrorMsg);
            });
        }


        protected static bool ValidityGreaterThanNow(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}
