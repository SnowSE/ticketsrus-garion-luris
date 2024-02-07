using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketsRUs.ClassLib.Data;

namespace TicketsRUs.WebApp.Data;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableEvent> AvailableEvents { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<UserTicket> UserTickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "azure")
            .HasPostgresExtension("pg_catalog", "pgaadauth");

        modelBuilder.Entity<AvailableEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("available_event_pkey");

            entity.ToTable("available_event");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .HasColumnName("email");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ticket_pkey");

            entity.ToTable("ticket");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Scanned).HasColumnName("scanned");
            entity.Property(e => e.Seat)
                .HasMaxLength(4)
                .HasColumnName("seat");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("event_id");
        });

        modelBuilder.Entity<UserTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_ticket_pkey");

            entity.ToTable("user_ticket");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");

            entity.HasOne(d => d.Client).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("user_ticket_client_id_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("user_ticket_ticket_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
