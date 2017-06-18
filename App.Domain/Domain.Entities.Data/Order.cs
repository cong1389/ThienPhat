using App.Core.Common;
using App.Domain.Entities.Brandes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
    public class Order : AuditableEntity<int>
    {
        public string Accessories
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string AppleId
        {
            get;
            set;
        }

        [ForeignKey("BrandId")]
        public virtual Brand Brand
        {
            get;
            set;
        }

        public int BrandId
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string CustomerCode
        {
            get;
            set;
        }

        public string CustomerIdNumber
        {
            get;
            set;
        }

        public string CustomerName
        {
            get;
            set;
        }

        public decimal? FixedFee
        {
            get;
            set;
        }

        public string FixedTags
        {
            get;
            set;
        }

        public string IcloudPassword
        {
            get;
            set;
        }

        public string Model
        {
            get;
            set;
        }

        public string ModelBrand
        {
            get;
            set;
        }

        public string Note
        {
            get;
            set;
        }

        public int? OldWarranty
        {
            get;
            set;
        }

        public string OrderCode
        {
            get;
            set;
        }

        public virtual ICollection<OrderGallery> OrderGalleries
        {
            get;
            set;
        }

        public virtual ICollection<OrderItem> OrderItems
        {
            get;
            set;
        }

        public string PasswordPhone
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        public string PhoneStatus
        {
            get;
            set;
        }

        public string SerialNumber
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public string StoreName
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

        public Order()
        {
        }
    }
}