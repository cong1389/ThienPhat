using Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace App.FakeEntity.Assessments
{
    public class AssessmentViewModel
    {
        public int Id
        {
            get;
            set;
        }

        [Display(Name = "BillNumber", ResourceType = typeof(FormUI))]
        public string BillNumber { get; set; }

        [Display(Name = "CusomterNumber", ResourceType = typeof(FormUI))]
        public string CusomterNumber
        {
            get;
            set;
        }

        [Display(Name = "Address", ResourceType = typeof(FormUI))]
        public string Address
        {
            get;
            set;
        }

        [Display(Name = "FullName", ResourceType = typeof(FormUI))]
        public string FullName
        {
            get;
            set;
        }

        [Display(Name = "Password", ResourceType = typeof(FormUI))]
        public string Password
        {
            get;
            set;
        }

        [Display(Name = "IdentityCard", ResourceType = typeof(FormUI))]
        public string IdentityCard
        {
            get;
            set;
        }

        [Display(Name = "Warranty", ResourceType = typeof(FormUI))]
        public int Warranty
        {
            get;
            set;
        }

        [Display(Name = "FromWarranty", ResourceType = typeof(FormUI))]
        public DateTime? FromWarranty
        {
            get;
            set;
        }

        [Display(Name = "ToWarranty", ResourceType = typeof(FormUI))]
        public DateTime? ToWarranty
        {
            get;
            set;
        }

        public int BrandId { get; set; }

        [Display(Name = "Branch", ResourceType = typeof(FormUI))]
        public string Branch
        {
            get;
            set;
        }

        [Display(Name = "Model", ResourceType = typeof(FormUI))]
        public string Model { get; set; }

        [Display(Name = "Imei", ResourceType = typeof(FormUI))]
        public string Imei { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(FormUI))]
        public string PhoneNumber { get; set; }

        [Display(Name = "AppleId", ResourceType = typeof(FormUI))]
        public string AppleId
        {
            get;
            set;
        }

        [Display(Name = "ICloudPassword", ResourceType = typeof(FormUI))]
        public string ICloudPassword
        {
            get;
            set;
        }

        [Display(Name = "StatusCurrent", ResourceType = typeof(FormUI))]
        public string StatusCurrent
        {
            get;
            set;
        }

        [Display(Name = "RepairType", ResourceType = typeof(FormUI))]
        public int RepairTypeId
        {
            get;
            set;
        }
        [Display(Name = "RepairStatus", ResourceType = typeof(FormUI))]
        public string RepairStatus
        {
            get;
            set;
        }

        [Display(Name = "Status", ResourceType = typeof(FormUI))]
        public int Status
        {
            get;
            set;
        }

        [Display(Name = "Store", ResourceType = typeof(FormUI))]
        public int StoreId
        {
            get;
            set;
        }

        [Display(Name = "Accessories", ResourceType = typeof(FormUI))]
        public string Accessories
        {
            get;
            set;
        }

        [Display(Name = "SourceLink", ResourceType = typeof(FormUI))]
        public string OtherLink
        {
            get;
            set;
        }

        [Display(Name = "Description", ResourceType = typeof(FormUI))]
        public string Description
        {
            get;
            set;
        }

        [Display(Name = "OrderDisplay", ResourceType = typeof(FormUI))]
        public int OrderDisplay
        {
            get;
            set;
        }

        [Display(Name = "ImageUrl", ResourceType = typeof(FormUI))]
        public HttpPostedFileBase Image
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public AssessmentViewModel()
        {
        }
    }
}