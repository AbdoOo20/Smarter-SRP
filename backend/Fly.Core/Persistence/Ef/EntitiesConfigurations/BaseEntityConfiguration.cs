using Fly.Framework.Server.Persistence.Common;
using Fly.Framework.Server.Persistence.ValueConverters;
using Fly.Framework.Server.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Fly.Core.Persistence.Ef.EntitiesConfigurations;

internal class BaseEntityConfiguration<XEntity> : IEntityTypeConfiguration<XEntity>
    where XEntity : TEntity
{

    public virtual void Configure(EntityTypeBuilder<XEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd()
            //.UseMySqlIdentityColumn()
            .HasConversion<IdentifierConverter>();

        builder
            .Property(x => x.Guid)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.CreateDate)
            .HasValueGenerator<MyDateGenerator>()
            .ValueGeneratedOnAdd();


        //builder
        //    .Property(x => x.UpdateDate)
        //    .HasValueGenerator<MyDateGenerator>()
        //    .ValueGeneratedOnUpdate()
        //;

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}

