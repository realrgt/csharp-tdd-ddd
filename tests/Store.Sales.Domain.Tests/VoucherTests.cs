using System;
using System.Linq;
using Xunit;

namespace Store.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Type Amount Valid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVouchersTypeAmount_ShouldBeValid()
        {
            // Arrange 
            var voucher = new Voucher(code: "PROMO-15-MZN", discountAmount: 15, discountPercent: null, DiscountType.Amount, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);
           
            // Act 
            var result = voucher.ValidateIsApplicable();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Type Amount Invalid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVouchersTypeAmount_ShouldBeInValid()
        {
            // Arrange 
            var voucher = new Voucher(code: "", discountAmount: null, discountPercent: null, DiscountType.Amount, quantity: 0, DateTime.Now.AddDays(-1), isActive: false, isUsed: true);

            // Act 
            var result = voucher.ValidateIsApplicable();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expected: 6, actual: result.Errors.Count);
            Assert.Contains(VoucherApplicableValidation.IsActiveErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.CodeErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.ValidityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.QuantityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.IsUsedErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.DiscountAmountErrorMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validate Voucher Type Percentage Valid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVouchersTypePercentage_ShouldBeValid()
        {
            // Arrange 
            var voucher = new Voucher(code: "PROMO-15-OFF", discountAmount: null, discountPercent: 15, DiscountType.Percentage, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);

            // Act 
            var result = voucher.ValidateIsApplicable();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Type Percentage Invalid")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVouchersTypePercentage_ShouldBeInValid()
        {
            // Arrange 
            var voucher = new Voucher(code: "", discountAmount: null, discountPercent: null, DiscountType.Percentage, quantity: 0, DateTime.Now.AddDays(-1), isActive: false, isUsed: true);

            // Act 
            var result = voucher.ValidateIsApplicable();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(expected: 6, actual: result.Errors.Count);
            Assert.Contains(VoucherApplicableValidation.IsActiveErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.CodeErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.ValidityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.QuantityErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.IsUsedErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicableValidation.DiscountPercentErrorMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
