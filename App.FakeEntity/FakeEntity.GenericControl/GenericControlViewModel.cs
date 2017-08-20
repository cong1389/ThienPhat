using App.Service.Language;
using Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.FakeEntity.GenericControl
{
    public class GenericControlViewModel : ILocalizedModel<GenericControlLocalesViewModel>
    {
		[Display(Name="Name", ResourceType=typeof(FormUI))]
		public string Name
		{
			get;
			set;
		}

		[Display(Name="Description", ResourceType=typeof(FormUI))]
		public string Description
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		[Display(Name="OrderDisplay", ResourceType=typeof(FormUI))]
		public int? OrderDisplay
		{
			get;
			set;
		}

		[Display(Name="Status", ResourceType=typeof(FormUI))]
		public int Status
		{
			get;
			set;
		}

        [Display(Name = "Entity", ResourceType = typeof(FormUI))]
        public int EntityId
        {
            get;
            set;
        }

        public int ControlTypeId { get; set; }

        public IList<GenericControlLocalesViewModel> Locales { get; set; }

        public GenericControlViewModel()
		{
            this.Locales = new List<GenericControlLocalesViewModel>();
        }
	}

    public class GenericControlLocalesViewModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        public int LocalesId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(FormUI))]
        public string Name
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

        public int Id
        {
            get;
            set;
        }

        [Display(Name = "OrderDisplay", ResourceType = typeof(FormUI))]
        public int? OrderDisplay
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

        [Display(Name = "Entity", ResourceType = typeof(FormUI))]
        public int EntityId
        {
            get;
            set;
        }

        public int ControlTypeId { get; set; }
    }
}