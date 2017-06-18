using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace App.Framework.Theme
{
    public abstract class ThemeableVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        private const string CacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";

        private const string CacheKeyPrefixMaster = "Master";

        private const string CacheKeyPrefixPartial = "Partial";

        private const string CacheKeyPrefixView = "View";

        private readonly static string[] _emptyLocations;

        internal Func<string, string> GetExtensionThunk = new Func<string, string>(VirtualPathUtility.GetExtension);

        static ThemeableVirtualPathProviderViewEngine()
        {
            ThemeableVirtualPathProviderViewEngine._emptyLocations = new string[0];
        }

        protected ThemeableVirtualPathProviderViewEngine()
        {
        }

        protected virtual new string AppendDisplayModeToCacheKey(string cacheKey, string displayMode)
        {
            return string.Concat(cacheKey, displayMode, ":");
        }

        protected virtual new string CreateCacheKey(string prefix, string name, string controllerName, string areaName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:", new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName });
        }

        protected virtual bool FilePathIsSupported(string virtualPath)
        {
            bool flag;
            if (base.FileExtensions != null)
            {
                string str = this.GetExtensionThunk(virtualPath).TrimStart(new char[] { '.' });
                flag = base.FileExtensions.Contains<string>(str, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            string[] strArrays;
            ViewEngineResult viewEngineResult;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Partial view name cannot be null or empty.", "partialViewName");
            }
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string path = this.GetPath(controllerContext, base.PartialViewLocationFormats, base.AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, requiredString, "Partial", useCache, out strArrays);
            viewEngineResult = (!string.IsNullOrEmpty(path) ? new ViewEngineResult(this.CreatePartialView(controllerContext, path), this) : new ViewEngineResult(strArrays));
            return viewEngineResult;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string[] strArrays;
            string[] strArrays1;
            ViewEngineResult viewEngineResult;
            bool flag;
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("View name cannot be null or empty.", "viewName");
            }
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string path = this.GetPath(controllerContext, base.ViewLocationFormats, base.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out strArrays);
            string str = this.GetPath(controllerContext, base.MasterLocationFormats, base.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out strArrays1);
            if (string.IsNullOrEmpty(path))
            {
                flag = true;
            }
            else
            {
                flag = (!string.IsNullOrEmpty(str) ? false : !string.IsNullOrEmpty(masterName));
            }
            viewEngineResult = (!flag ? new ViewEngineResult(this.CreateView(controllerContext, path, str), this) : new ViewEngineResult(strArrays.Union<string>(strArrays1)));
            return viewEngineResult;
        }

        protected virtual string GetAreaName(RouteBase route)
        {
            string item;
            IRouteWithArea routeWithArea = route as IRouteWithArea;
            if (routeWithArea == null)
            {
                Route route1 = route as Route;
                if ((route1 == null ? true : route1.DataTokens == null))
                {
                    item = null;
                }
                else
                {
                    item = route1.DataTokens["area"] as string;
                }
            }
            else
            {
                item = routeWithArea.Area;
            }
            return item;
        }

        protected virtual string GetAreaName(RouteData routeData)
        {
            object obj;
            string str;
            str = (!routeData.DataTokens.TryGetValue("area", out obj) ? this.GetAreaName(routeData.Route) : obj as string);
            return str;
        }

        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            string empty;
            string[] strArrays;
            searchedLocations = ThemeableVirtualPathProviderViewEngine._emptyLocations;
            if (!string.IsNullOrEmpty(name))
            {
                string areaName = this.GetAreaName(controllerContext.RouteData);
                if ((string.IsNullOrEmpty(areaName) ? false : areaName.Equals("admin", StringComparison.InvariantCultureIgnoreCase)))
                {
                    List<string> list = areaLocations.ToList<string>();
                    list.Insert(0, "~/Areas/Admin/Views/{1}/{0}.cshtml");
                    list.Insert(0, "~/Areas/Admin/Views/Shared/{0}.cshtml");
                    areaLocations = list.ToArray();
                }
                bool flag = !string.IsNullOrEmpty(areaName);
                string[] strArrays1 = locations;
                if (flag)
                {
                    strArrays = areaLocations;
                }
                else
                {
                    strArrays = null;
                }
                List<ThemeableVirtualPathProviderViewEngine.ViewLocation> viewLocations = this.GetViewLocations(strArrays1, strArrays);
                if (viewLocations.Count == 0)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty - {0}", new object[] { locationsPropertyName }));
                }
                bool flag1 = this.IsSpecificPath(name);
                string str = this.CreateCacheKey(cacheKeyPrefix, name, (flag1 ? string.Empty : controllerName), areaName);
                if (!useCache)
                {
                    empty = (flag1 ? this.GetPathFromSpecificName(controllerContext, name, str, ref searchedLocations) : this.GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, str, ref searchedLocations));
                }
                else
                {
                    foreach (IDisplayMode availableDisplayModesForContext in base.DisplayModeProvider.GetAvailableDisplayModesForContext(controllerContext.HttpContext, controllerContext.DisplayMode))
                    {
                        string viewLocation = base.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, this.AppendDisplayModeToCacheKey(str, availableDisplayModesForContext.DisplayModeId));
                        if (viewLocation == null)
                        {
                        }
                        if ((viewLocation == null ? false : viewLocation.Length > 0))
                        {
                            if (controllerContext.DisplayMode == null)
                            {
                                controllerContext.DisplayMode = availableDisplayModesForContext;
                            }
                            empty = viewLocation;
                            return empty;
                        }
                    }
                    empty = null;
                }
            }
            else
            {
                empty = string.Empty;
            }
            return empty;
        }

        protected virtual string GetPathFromGeneralName(ControllerContext controllerContext, List<ThemeableVirtualPathProviderViewEngine.ViewLocation> locations, string name, string controllerName, string areaName, string cacheKey, ref string[] searchedLocations)
        {
            Func<string, bool> func;
            Func<string, bool> func1 = null;
            Func<string, bool> func2 = null;
            string empty = string.Empty;
            searchedLocations = new string[locations.Count];
            int num = 0;
            while (num < locations.Count)
            {
                ThemeableVirtualPathProviderViewEngine.ViewLocation item = locations[num];
                string str = "";
                str = item.Format(name, controllerName, areaName);
                if (!File.Exists(HttpContext.Current.Server.MapPath(str)))
                {
                    str = item.Format(name, controllerName, areaName);
                }
                System.Web.WebPages.DisplayModeProvider displayModeProvider = base.DisplayModeProvider;
                string str1 = str;
                HttpContextBase httpContext = controllerContext.HttpContext;
                Func<string, bool> func3 = func1;
                if (func3 == null)
                {
                    Func<string, bool> func4 = (string path) => this.FileExists(controllerContext, path);
                    func = func4;
                    func1 = func4;
                    func3 = func;
                }
                DisplayInfo displayInfoForVirtualPath = displayModeProvider.GetDisplayInfoForVirtualPath(str1, httpContext, func3, controllerContext.DisplayMode);
                if (displayInfoForVirtualPath == null)
                {
                    searchedLocations[num] = str;
                    num++;
                }
                else
                {
                    string filePath = displayInfoForVirtualPath.FilePath;
                    searchedLocations = ThemeableVirtualPathProviderViewEngine._emptyLocations;
                    empty = filePath;
                    base.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, this.AppendDisplayModeToCacheKey(cacheKey, displayInfoForVirtualPath.DisplayMode.DisplayModeId), empty);
                    if (controllerContext.DisplayMode == null)
                    {
                        controllerContext.DisplayMode = displayInfoForVirtualPath.DisplayMode;
                    }
                    foreach (IDisplayMode mode in base.DisplayModeProvider.Modes)
                    {
                        if (mode.DisplayModeId != displayInfoForVirtualPath.DisplayMode.DisplayModeId)
                        {
                            IDisplayMode displayMode = mode;
                            HttpContextBase httpContextBase = controllerContext.HttpContext;
                            string str2 = str;
                            Func<string, bool> func5 = func2;
                            if (func5 == null)
                            {
                                Func<string, bool> func6 = (string path) => this.FileExists(controllerContext, path);
                                func = func6;
                                func2 = func6;
                                func5 = func;
                            }
                            DisplayInfo displayInfo = displayMode.GetDisplayInfo(httpContextBase, str2, func5);
                            string empty1 = string.Empty;
                            if ((displayInfo == null ? false : displayInfo.FilePath != null))
                            {
                                empty1 = displayInfo.FilePath;
                            }
                            base.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, this.AppendDisplayModeToCacheKey(cacheKey, mode.DisplayModeId), empty1);
                        }
                    }
                    break;
                }
            }
            return empty;
        }

        protected virtual string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string empty = name;
            if ((!this.FilePathIsSupported(name) ? true : !this.FileExists(controllerContext, name)))
            {
                empty = string.Empty;
                searchedLocations = new string[] { name };
            }
            base.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, empty);
            return empty;
        }

        protected virtual List<ThemeableVirtualPathProviderViewEngine.ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            List<ThemeableVirtualPathProviderViewEngine.ViewLocation> viewLocations = new List<ThemeableVirtualPathProviderViewEngine.ViewLocation>();
            if (areaViewLocationFormats != null)
            {
                string[] strArrays = areaViewLocationFormats;
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    viewLocations.Add(new ThemeableVirtualPathProviderViewEngine.AreaAwareViewLocation(strArrays[i]));
                }
            }
            if (viewLocationFormats != null)
            {
                string[] strArrays1 = viewLocationFormats;
                for (int j = 0; j < (int)strArrays1.Length; j++)
                {
                    viewLocations.Add(new ThemeableVirtualPathProviderViewEngine.ViewLocation(strArrays1[j]));
                }
            }
            return viewLocations;
        }

        protected virtual bool IsSpecificPath(string name)
        {
            char chr = name[0];
            return (chr == '~' ? true : chr == '/');
        }

        public class AreaAwareViewLocation : ThemeableVirtualPathProviderViewEngine.ViewLocation
        {
            public AreaAwareViewLocation(string virtualPathFormatString) : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string controllerName, string areaName)
            {
                return string.Format(CultureInfo.InvariantCulture, this._virtualPathFormatString, new object[] { viewName, controllerName, areaName });
            }
        }

        public class ViewLocation
        {
            protected readonly string _virtualPathFormatString;

            public ViewLocation(string virtualPathFormatString)
            {
                this._virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName)
            {
                return string.Format(CultureInfo.InvariantCulture, this._virtualPathFormatString, new object[] { viewName, controllerName });
            }
        }
    }
}