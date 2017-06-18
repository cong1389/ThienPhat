using App.Core.Common;
using System;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Data
{
    public class Assessment : AuditableEntity<int>
    {
        #region fields

        private string billNumber=string.Empty;
        private string cusomterNumber = string.Empty;
        private string address;
        private string fullName;
        private string password;
        private string identityCard;
        private int warranty;      
        private int brandId;
        private string branch;
        private string model;
        private string imei;
        private string phoneNumber;
        private string appleId;
        private string iCloudPassword;
        private string statusCurrent;
        private string repairStatus;
        private int repairTypeId;
        private int status;
        private int storeId;
        private string accessories;
        private string otherLink;
        private string description;
        private int orderDisplay;
        private string imageUrl;
        #endregion

        #region properties

        public string BillNumber
        {
            get { return this.billNumber; }
            set { this.billNumber = value; }
        }
        public string CusomterNumber
        {
            get { return this.cusomterNumber; }
            set { this.cusomterNumber = value; }
        }
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }
        public string FullName
        {
            get { return this.fullName; }
            set { this.fullName = value; }
        }
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }
        public string IdentityCard
        {
            get { return this.identityCard; }
            set { this.identityCard = value; }
        }
        public int Warranty
        {
            get { return this.warranty; }
            set { this.warranty = value; }
        }
        public DateTime? FromWarranty
        {
            get;
            set;
        }
        public DateTime? ToWarranty
        {
            get;
            set;
        }
        public int BrandId
        {
            get { return this.brandId; }
            set { this.brandId = value; }
        }
        public string Branch
        {
            get { return this.branch; }
            set { this.branch = value; }
        }
        public string Model
        {
            get { return this.model; }
            set { this.model = value; }
        }
        public string Imei
        {
            get { return this.imei; }
            set { this.imei = value; }
        }
        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set { this.phoneNumber = value; }
        }
        public string AppleId
        {
            get { return this.appleId; }
            set { this.appleId = value; }
        }
        public string ICloudPassword
        {
            get { return this.iCloudPassword; }
            set { this.iCloudPassword = value; }
        }
        public string StatusCurrent
        {
            get { return this.statusCurrent; }
            set { this.statusCurrent = value; }
        }
        public int RepairTypeId
        {
            get { return this.repairTypeId; }
            set { this.repairTypeId = value; }
        }
        public string RepairStatus
        {
            get { return this.repairStatus; }
            set { this.repairStatus = value; }
        }
        public int Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        public int StoreId
        {
            get { return this.storeId; }
            set { this.storeId = value; }
        }
        public string Accessories
        {
            get { return this.accessories; }
            set { this.accessories = value; }
        }
        public string OtherLink
        {
            get { return this.otherLink; }
            set { this.otherLink = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public int OrderDisplay
        {
            get { return this.orderDisplay; }
            set { this.orderDisplay = value; }
        }
        public string ImageUrl
        {
            get { return this.imageUrl; }
            set { this.imageUrl = value; }
        }

        #endregion

        public Assessment()
        {
        }
    }
}