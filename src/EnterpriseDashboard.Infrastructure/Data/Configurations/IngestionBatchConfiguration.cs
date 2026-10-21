using System;
using EnterpriseDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnterpriseDashboard.Infrastructure.Data.Configurations;

public class IngestionBatchConfiguration : IEntityTypeConfiguration<IngestionBatch>
{
    public void Configure(EntityTypeBuilder<IngestionBatch> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(b => b.ErrorSummary)
            .HasMaxLength(2000);
    }
}
