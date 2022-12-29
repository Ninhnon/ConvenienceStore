using ConvenienceStore.Model.Admin;
using FluentValidation;

namespace ConvenienceStore.Utils.Validation
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Chưa nhập tên nhà cung cấp");

            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Chưa nhập địa chỉ NCC");

            RuleFor(p => p.Phone)
                .Must(BeAValidPhoneNumber).WithMessage("SĐT không hợp lệ");
        }

        protected bool BeAValidPhoneNumber(string Phone)
        {
            int t = 0;
            for (int i = 0; i < Phone.Length; i++)
            {
                if (Phone[i] != ' ' && Phone[i] != '(' && Phone[i] != ')' && !int.TryParse(Phone[i].ToString(), out t))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
