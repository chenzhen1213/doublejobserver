using System;
using System.Collections.Generic;
using System.Text;
using DoubleJobServer.Core.DomainModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

using System.Linq;

namespace DoubleJobServer.Infrastructure
{
    public class MyContextSeed
    {
        public static async Task SeedAsync(MyContext myContext, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;

            try
            {
                if (!myContext.Countries.Any())
                {
                    myContext.Countries.AddRange(
                        new List<Country>{
                            new Country{
                                Id =0,
                                EnglishName = $"China",
                                ChineseName = $"中华人民共和国",
                                Abbreviation = $"中国"
                            },
                            new Country{
                                Id =2,
                                EnglishName = $"USA",
                                ChineseName = $"美利坚合众国",
                                Abbreviation = $"美国"
                            },
                            new Country{
                                Id =4,
                                EnglishName = $"Finland",
                                ChineseName = $"芬兰共和国",
                                Abbreviation = $"芬兰"
                            },
                            new Country{
                                Id =6,
                                EnglishName = "UK",
                                ChineseName = "大不列颠",
                                Abbreviation = "英国"
                            }
                         }
                    );

                    myContext.Cities.AddRange(
                        new List<City>{
                            new City{
                                CountryId = 1,
                                Name = "北京"
                            },
                            new City{
                                CountryId = 1,
                                Name = "上海"
                            },
                            new City{
                                CountryId = 1,
                                Name = "深圳"
                            },
                            new City{
                                CountryId = 1,
                                Name = "杭州"
                            },
                            new City{
                                CountryId  = 1,
                                Name = "天津"
                            },
                            new City
                            {
                                CountryId= 2,
                                Name = "New York"
                            }
                        }
                        );

                    await myContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<MyContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(myContext, loggerFactory, retryForAvailability);
                }

            }

        }

    }
}
