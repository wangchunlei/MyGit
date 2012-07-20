﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.Web.Tools.UI.Form
{
    public interface IFormModel<T> where T:class 
    {
        IFromRenderer<T> Renderer { get; set; }
        IDictionary<string, object> Attributes { get; set; }
        String SortPrefix { get; set; }
        IFormLayout FormLayout { get;  set; }
    }
}
