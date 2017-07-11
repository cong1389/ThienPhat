using App.Domain.Entities.Language;
using App.FakeEntity.Language;
using App.Service.Language;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Front.Models
{
    public class MenuNav : ILocalizedModel<MenuNavLocales>
    {
        public List<MenuNav> ChildNavMenu
        {
            get;
            set;
        }

        public string CurrentVirtualId
        {
            get;
            set;
        }

        public string IconBar
        {
            get;
            set;
        }

        public string IconNav
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public int MenuId
        {
            get;
            set;
        }

        public string MenuName
        {
            get;
            set;
        }

        public int OrderDisplay
        {
            get;
            set;
        }

        public string OtherLink
        {
            get;
            set;
        }

        public int? ParentId
        {
            get;
            set;
        }

        public string SeoUrl
        {
            get;
            set;
        }

        public int TemplateType
        {
            get;
            set;
        }

        public string VirtualId
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }
        public IList<MenuNavLocales> Locales { get; set; }

        public MenuNav()
        {
            this.Locales = new List<MenuNavLocales>();
        }
    }

    public class MenuNavLocales : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        public int LocalesId { get; set; }

        public string MenuName
        {
            get;
            set;
        }
    }
}