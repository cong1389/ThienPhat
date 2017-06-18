using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.Order
{
    public class OrderItemViewModel
    {
        [Display(Name = "Hạng mục sửa chữa")]
        public string Category
        {
            get;
            set;
        }

        [Display(Name = "Chi phí")]
        public decimal? FixedFee
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public string Index
        {
            get;
            set;
        }

        public OrderViewModel Order
        {
            get;
            set;
        }

        public int OrderId
        {
            get;
            set;
        }

        public DateTime? WarrantyFrom
        {
            get;
            set;
        }

        public DateTime? WarrantyTo
        {
            get;
            set;
        }

        public OrderItemViewModel()
        {
        }
    }
}