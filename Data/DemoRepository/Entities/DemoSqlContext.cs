using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoRepository.Entities;

public partial class DemoSqlContext : DbContext
{
    public DemoSqlContext(DbContextOptions<DemoSqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientAudit> ClientAudits { get; set; }

    public virtual DbSet<ClientAuditView> ClientAuditViews { get; set; }

    public virtual DbSet<ClientUser> ClientUsers { get; set; }

    public virtual DbSet<ClientUserAudit> ClientUserAudits { get; set; }

    public virtual DbSet<ClientUserAuditView> ClientUserAuditViews { get; set; }

    public virtual DbSet<ClientUserView> ClientUserViews { get; set; }

    public virtual DbSet<ClientView> ClientViews { get; set; }

    public virtual DbSet<DataDictionary> DataDictionaries { get; set; }

    public virtual DbSet<DataDictionaryAudit> DataDictionaryAudits { get; set; }

    public virtual DbSet<DataDictionaryAuditView> DataDictionaryAuditViews { get; set; }

    public virtual DbSet<DataDictionaryGroup> DataDictionaryGroups { get; set; }

    public virtual DbSet<DataDictionaryGroupAudit> DataDictionaryGroupAudits { get; set; }

    public virtual DbSet<DataDictionaryGroupAuditView> DataDictionaryGroupAuditViews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAudit> UserAudits { get; set; }

    public virtual DbSet<UserAuditView> UserAuditViews { get; set; }

    public virtual DbSet<UserView> UserViews { get; set; }

    public virtual DbSet<WorkItem> WorkItems { get; set; }

    public virtual DbSet<WorkItemAudit> WorkItemAudits { get; set; }

    public virtual DbSet<WorkItemAuditView> WorkItemAuditViews { get; set; }

    public virtual DbSet<WorkItemUser> WorkItemUsers { get; set; }

    public virtual DbSet<WorkItemUserAudit> WorkItemUserAudits { get; set; }

    public virtual DbSet<WorkItemUserView> WorkItemUserViews { get; set; }

    public virtual DbSet<WorkItemView> WorkItemViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.ClientGuid).HasDefaultValueSql("NEWID()");

            entity.HasOne(d => d.ClientType).WithMany(p => p.Clients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_DataDictionary");
        });

        modelBuilder.Entity<ClientAudit>(entity =>
        {
            entity.Property(e => e.ClientAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.ClientAuditAction).WithMany(p => p.ClientAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAudit_DataDictionary");

            entity.HasOne(d => d.ClientAuditClient).WithMany(p => p.ClientAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAudit_Client");

            entity.HasOne(d => d.ClientAuditUser).WithMany(p => p.ClientAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientAudit_User");
        });

        modelBuilder.Entity<ClientAuditView>(entity =>
        {
            entity.ToView("ClientAuditView");
        });

        modelBuilder.Entity<ClientUser>(entity =>
        {
            entity.HasOne(d => d.ClientUserClient).WithMany(p => p.ClientUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUser_Client");

            entity.HasOne(d => d.ClientUserUser).WithMany(p => p.ClientUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUser_User");
        });

        modelBuilder.Entity<ClientUserAudit>(entity =>
        {
            entity.Property(e => e.ClientUserAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.ClientUserAuditAction).WithMany(p => p.ClientUserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUserAudit_DataDictionary");

            entity.HasOne(d => d.ClientUserAuditClientUser).WithMany(p => p.ClientUserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUserAudit_ClientUser");

            entity.HasOne(d => d.ClientUserAuditUser).WithMany(p => p.ClientUserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUserAudit_User");
        });

        modelBuilder.Entity<ClientUserAuditView>(entity =>
        {
            entity.ToView("ClientUserAuditView");
        });

        modelBuilder.Entity<ClientUserView>(entity =>
        {
            entity.ToView("ClientUserView");
        });

        modelBuilder.Entity<ClientView>(entity =>
        {
            entity.ToView("ClientView");
        });

        modelBuilder.Entity<DataDictionary>(entity =>
        {
            entity.HasOne(d => d.DataDictionaryGroup).WithMany(p => p.DataDictionaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionary_DataDictionarGroup");
        });

        modelBuilder.Entity<DataDictionaryAudit>(entity =>
        {
            entity.Property(e => e.DataDictionaryAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.DataDictionaryAuditAction).WithMany(p => p.DataDictionaryAuditDataDictionaryAuditActions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionaryAudit_DataDictionary_ActionId");

            entity.HasOne(d => d.DataDictionaryAuditDataDictionary).WithMany(p => p.DataDictionaryAuditDataDictionaryAuditDataDictionaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionaryAudit_DataDictionary_DataDictionaryId");

            entity.HasOne(d => d.DataDictionaryAuditUser).WithMany(p => p.DataDictionaryAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionaryAudit_User");
        });

        modelBuilder.Entity<DataDictionaryAuditView>(entity =>
        {
            entity.ToView("DataDictionaryAuditView");
        });

        modelBuilder.Entity<DataDictionaryGroupAudit>(entity =>
        {
            entity.Property(e => e.DataDictionaryGroupAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.DataDictionaryGroupAuditAction).WithMany(p => p.DataDictionaryGroupAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionaryGroupAudit_DataDictionary");

            entity.HasOne(d => d.DataDictionaryGroupAuditDataDictionaryGroup).WithMany(p => p.DataDictionaryGroupAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionaryGroupAudit_DataDictionaryGroup");

            entity.HasOne(d => d.DataDictionaryGroupAuditUser).WithMany(p => p.DataDictionaryGroupAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionaryGroupAudit_User");
        });

        modelBuilder.Entity<DataDictionaryGroupAuditView>(entity =>
        {
            entity.ToView("DataDictionaryGroupAuditView");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserGuid).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.UserPasswordAttemptCount).HasDefaultValue(-1);

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_DataDictionary");
        });

        modelBuilder.Entity<UserAudit>(entity =>
        {
            entity.Property(e => e.UserAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.UserAuditAction).WithMany(p => p.UserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAudit_DataDictionary");

            entity.HasOne(d => d.UserAuditUser).WithMany(p => p.UserAuditUserAuditUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAudit_User");

            entity.HasOne(d => d.UserAuditUserIdSourceNavigation).WithMany(p => p.UserAuditUserAuditUserIdSourceNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAudit_User_Source");
        });

        modelBuilder.Entity<UserAuditView>(entity =>
        {
            entity.ToView("UserAuditView");
        });

        modelBuilder.Entity<UserView>(entity =>
        {
            entity.ToView("UserView");
        });

        modelBuilder.Entity<WorkItem>(entity =>
        {
            entity.Property(e => e.WorkItemGuid).HasDefaultValueSql("NEWID()");

            entity.HasOne(d => d.WorkItemClient).WithMany(p => p.WorkItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItem_Client");

            entity.HasOne(d => d.WorkItemStatus).WithMany(p => p.WorkItemWorkItemStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItem_DataDictionary_Status");

            entity.HasOne(d => d.WorkItemType).WithMany(p => p.WorkItemWorkItemTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItem_DataDictionary_Type");
        });

        modelBuilder.Entity<WorkItemAudit>(entity =>
        {
            entity.Property(e => e.WorkItemAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.WorkItemAuditAction).WithMany(p => p.WorkItemAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemAudit_DataDictionary");

            entity.HasOne(d => d.WorkItemAuditUser).WithMany(p => p.WorkItemAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemAudit_User");

            entity.HasOne(d => d.WorkItemAuditWorkItem).WithMany(p => p.WorkItemAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemAudit_WorkItem");
        });

        modelBuilder.Entity<WorkItemAuditView>(entity =>
        {
            entity.ToView("WorkItemAuditView");
        });

        modelBuilder.Entity<WorkItemUser>(entity =>
        {
            entity.HasOne(d => d.WorkItemUserUser).WithMany(p => p.WorkItemUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemUser_User");

            entity.HasOne(d => d.WorkItemUserWorkItem).WithMany(p => p.WorkItemUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemUser_WorkItem");
        });

        modelBuilder.Entity<WorkItemUserAudit>(entity =>
        {
            entity.Property(e => e.WorkItemUserAuditDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.WorkItemUserAuditAction).WithMany(p => p.WorkItemUserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemUserAudit_DataDictionary");

            entity.HasOne(d => d.WorkItemUserAuditUser).WithMany(p => p.WorkItemUserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemUserAudit_User");

            entity.HasOne(d => d.WorkItemUserAuditWorkItemUser).WithMany(p => p.WorkItemUserAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkItemUserAudit_WorkItemUser");
        });

        modelBuilder.Entity<WorkItemUserView>(entity =>
        {
            entity.ToView("WorkItemUserView");
        });

        modelBuilder.Entity<WorkItemView>(entity =>
        {
            entity.ToView("WorkItemView");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
