using System;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.Order
{
    public class OrderGalleryViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public int OrderId
        {
            get;
            set;
        }

        public OrderGalleryViewModel()
        {
        }
    }
}