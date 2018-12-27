﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using RestSupplyDB;
using RestSupplyMVC.Repositories;

namespace RestSupplyMVC.Persistence
{
    public class UnitOfWork
    {
        private readonly RestSupplyDBModel _context;

        public SupplierRepository Suppliers { get; private set; }
        public UnitOfWork(RestSupplyDBModel context)
        {
            _context = context;
            Suppliers = new SupplierRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}