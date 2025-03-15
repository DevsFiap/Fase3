using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fase03.Domain.Entities;
using Fase03.Domain.Enums;

namespace Fase03.Infra.Data.Configuration
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {
            builder.ToTable("Contato");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.Telefone).HasColumnType("VARCHAR(25)").IsRequired();
            builder.Property(u => u.Email).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(u => u.DDDTelefone).HasColumnType("int").IsRequired();
            builder.Property(u => u.DataCriacao).HasColumnType("DATETIME").IsRequired();

    }
    }
}
