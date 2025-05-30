﻿using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess.MongoDb;
public class MongoCategoryData : ICategoryData
{
   private const string CacheName = "CategoryData";
   private readonly IMemoryCache _cache;
   private readonly IMongoCollection<CategoryModel> _categories;

   public MongoCategoryData(IDbConnection db, IMemoryCache cache)
   {
      _cache = cache;
      _categories = db.CategoryCollection;
   }

   public async Task<List<CategoryModel>> GetAllCategories()
   {
      var output = _cache.Get<List<CategoryModel>>(CacheName);

      if (output == null || output.Count == 0)
      {
         var results = await _categories.FindAsync(_ => true);
         output = results.ToList();

         _cache.Set(CacheName, output, TimeSpan.FromDays(1));
      }

      return output;
   }

   public Task CreateCategory(CategoryModel category)
   {
      // We could invalidate the cashe here.
      // However, right now, end user wont be able to access this.
      return _categories.InsertOneAsync(category);
   }
}
