﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RestSupplyDB;
using RestSupplyDB.Models.Ingredient;
using RestSupplyDB.Models.Kitchen;

namespace RestSupplyMVC.Repositories
{
    public class KitchenIngredientRepository : IKitchenIngredientRepository
    {
        private readonly RestSupplyDbContext _context;
        public KitchenIngredientRepository(RestSupplyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<KitchenIngredients> GetAll()
        {
            return _context.KitchenIngredientsSet.ToList();
        }

        public KitchenIngredients GetById<TKey>(TKey id)
        {
            var res = _context.KitchenIngredientsSet.Find(id);
            return res;
        }



        public void Add(KitchenIngredients item)
        {
            _context.KitchenIngredientsSet.Add(item);
        }
        
        public void Remove(KitchenIngredients item)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<int, KitchenIngredients> GetIngredientIdToKitchenIngredientMap(int kitchenId)
        {
            var kitchenIngredients = _context.KitchenIngredientsSet.Include(k => k.IngredientsSet).Include(k => k.KitchensSet).Where(k => k.KitchenId == kitchenId);

            var map = new Dictionary<int,KitchenIngredients>();
            foreach (var ingredient in kitchenIngredients)
            {
                map.Add(ingredient.IngredientId, ingredient);
            }

            return map;

        }

        public KitchenIngredients GetByKitchenAndIngredientIds(int kitchenId, int ingredientId)
        {
            var kitchenIngredient =
                _context.KitchenIngredientsSet.FirstOrDefault(ki =>
                    ki.KitchenId == kitchenId && ki.IngredientId == ingredientId);
            return kitchenIngredient;
        }
    }
}