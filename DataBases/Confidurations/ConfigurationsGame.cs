using DataBases.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBases.Confidurations
{
    public class ConfigurationsGame : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.HasIndex(p => p.Name, "IX_Game_Name")
                .IsUnique();
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
