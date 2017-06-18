using App.FakeEntity.Order;
using FluentValidation;
using System;
using System.Linq.Expressions;

namespace App.Framework.ValidateEntity
{
    public class OrderValidator : AbstractValidator<OrderViewModel>
    {
        public OrderValidator()
        {
            base.RuleFor<string>((OrderViewModel x) => x.CustomerName).NotEmpty<OrderViewModel, string>().WithMessage<OrderViewModel, string>("Vui lòng nhập tên bạn.");
            base.RuleFor<int>((OrderViewModel x) => x.BrandId).NotEmpty<OrderViewModel, int>().WithMessage<OrderViewModel, int>("Vui lòng chọn thương hiệu.");
            base.RuleFor<string>((OrderViewModel x) => x.PhoneNumber).NotEmpty<OrderViewModel, string>().WithMessage<OrderViewModel, string>("Vui lòng nhập số điện thoại.");
            base.RuleFor<string>((OrderViewModel x) => x.ModelBrand).NotEmpty<OrderViewModel, string>().WithMessage<OrderViewModel, string>("Vui lòng nhập dòng máy.");
        }
    }
}