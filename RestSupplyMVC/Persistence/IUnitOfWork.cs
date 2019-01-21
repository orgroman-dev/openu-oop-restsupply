﻿using RestSupplyMVC.Repositories;

namespace RestSupplyMVC.Persistence
{
    public interface IUnitOfWork
    {
        ISupplierRepository Suppliers { get; }
        IIngredientRepository Ingredients { get; }
        IMenuItemsRepository MenuItems { get; }
        void Complete();
    }
}