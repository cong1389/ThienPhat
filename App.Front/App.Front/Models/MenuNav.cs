using App.Domain.Entities.Language;
using App.FakeEntity.Language;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Front.Models
{
    public class MenuNav
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

        public MenuNav()
        {
        }
    }

    public class MenuNavLocalized
    {
        public string MenuName
        {
            get;
            set;
        }
        public MenuNav MenuNavTranslation { get; set; }
        public LocalizedProperty LocalizedTranslation { get; set; }
    }
}