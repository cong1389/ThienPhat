using App.Core.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
    public class OrderGallery : Entity<int>
    {
        public string ImagePath
        {
            get;
            set;
        }

        [ForeignKey("OrderId")]
        public virtual Order Order
        {
            get;
            set;
        }

        public int OrderId
        {
            get;
            set;
        }

        public OrderGallery()
        {
        }
    }
}