using ConvenienceStore.Model.Admin;
using ConvenienceStore.Model.Staff;
using FluentValidation;

namespace ConvenienceStore.Utils.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Barcode)
                .NotEmpty().WithMessage("Chưa nhập mã vạch")
                .MinimumLength(8).WithMessage("Mã vạch có ít nhất 8 chữ số")
                .MaximumLength(13).WithMessage("Mã vạch có tối đa 13 chữ số");

            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Chưa nhập tên sản phẩm");

            RuleFor(p => p.Cost)
                .Must(NonNegativeInteger).WithMessage("Giá nhập phải >= 0");

            RuleFor(p => p.Price)
                .Must(NonNegativeInteger).WithMessage("Giá bán phải >= 0");

            RuleFor(p => p.Discount)
                .Must(NonNegativeDouble).WithMessage("Tỉ lệ giảm giá phải >= 0")
                .Must(BeAValidDiscount).WithMessage("Tỉ lệ giảm giá phải <= 100");

        }

        protected bool NonNegativeInteger(int Number)
        {
            return Number >= 0;
        }

        protected bool NonNegativeDouble(double Number)
        {
            return Number >= 0;
        }

        protected bool BeAValidDiscount(double discount)
        {
            return discount <= 100;
        }

    }
    public class ReportValidator : AbstractValidator<Report>
    {
        public ReportValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Chưa nhập tên vấn đề");
            RuleFor(p => p.RepairCost)
                .Must(NonNegativeInteger).WithMessage("Chi phí dự kiến phải >= 0");
        }

        protected bool NonNegativeInteger(int Number)
        {
            return Number >= 0;
        }

    }
}