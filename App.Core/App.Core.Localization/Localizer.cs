﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Localization
{
    public delegate LocalizedString Localizer(string key, params object[] args);
}
