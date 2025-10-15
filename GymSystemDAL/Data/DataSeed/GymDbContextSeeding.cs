using GymSystemDAL.Data.Context;
using GymSystemDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymSystemDAL.Data.DataSeed
{
    public class GymDbContextSeeding
    {
        public static bool SeedData(GymSystemDbContext dbContext)
        {
			try
			{
				var HasPlans = dbContext.Plans.Any();
				var HasCategories = dbContext.Categories.Any();
				if (HasPlans && HasCategories) return false;

				if (!HasPlans)
				{
					var Plans = LoadDataFromJsonFile<Plan>("plans.json");
					if (Plans.Any())
						dbContext.Plans.AddRange(Plans);
                }
                if (!HasCategories)
				{
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any())
                        dbContext.Categories.AddRange(Categories);
                }
				return dbContext.SaveChanges() > 0;
            }
			catch (Exception)
			{

				throw;
			}
        }
		private static List<T> LoadDataFromJsonFile<T>(string FileName)
		{
			var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FileName);

			if (File.Exists(FilePath))
			{
				throw new FileNotFoundException("File not found", FilePath);
            }
			string Data = File.ReadAllText(FilePath);
			var Options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true,
			};
			return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
