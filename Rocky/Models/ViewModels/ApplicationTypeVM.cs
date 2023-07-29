﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Rocky.Models.ViewModels
{
    public class ApplicationTypeVM
    {
        public ApplicationType ApplicationType { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
    }
}