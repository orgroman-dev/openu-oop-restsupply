﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestSupplyMVC.ViewModels
{
    public class SupplierListViewModel
    {
        public List<SupplierViewModel> SupplierList { get; set; }

        public SupplierListViewModel()
        { 
            SupplierList = new List<SupplierViewModel>();
        }
    }
}