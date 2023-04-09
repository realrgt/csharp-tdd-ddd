using System;
using Xunit;

namespace Store.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Type Valid Value")]
        [Trait("Category", "Sales - Voucher")]
        public void Voucher_ValidateVouchersTypeValue_ShouldBeValid()
        {
            // Arrange 
            var voucher = new Voucher(code: "PROMO-15-MZN", discountAmount: 15, discountPercent: null, DiscountType.Amount, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);
           
            // Act 
            var result = voucher.ValidateIsApplicable();

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
