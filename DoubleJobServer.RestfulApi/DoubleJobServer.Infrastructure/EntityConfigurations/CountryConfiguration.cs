using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DoubleJobServer.Core.DomainModels;

//为EFCore的model添加约束，这里我添加上（由于我使用的是内存数据库，所以下面的约束是不起作用的，这些约束只有在关系型数据库才起作用
//对于EFCore的实体约束和验证，我不愿意使用注解的方式（因为Model类应该只干自己的活），更喜欢使用fluent api

namespace DoubleJobServer.Infrastructure.EntityConfigurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(x => x.EnglishName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ChineseName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Abbreviation).HasMaxLength(5);
        }
    }
}
