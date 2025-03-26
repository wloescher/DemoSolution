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

    public virtual DbSet<WorkItemView> WorkItemViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__tmp_ms_x__E67E1A24AC0A634A");

            entity.Property(e => e.ClientGuid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.ClientType).WithMany(p => p.Clients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_DataDictionary");
        });

        modelBuilder.Entity<ClientAudit>(entity =>
        {
            entity.HasKey(e => e.ClientAuditId).HasName("PK__ClientAu__9DB057FC556648CC");

            entity.Property(e => e.ClientAuditDate).HasDefaultValueSql("(getdate())");

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
            entity.HasKey(e => e.ClientUserId).HasName("PK__tmp_ms_x__C46AAAF9D82EDAF9");

            entity.HasOne(d => d.ClientUserClient).WithMany(p => p.ClientUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUser_Client");

            entity.HasOne(d => d.ClientUserUser).WithMany(p => p.ClientUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientUser_User");
        });

        modelBuilder.Entity<ClientUserAudit>(entity =>
        {
            entity.HasKey(e => e.ClientUserAuditId).HasName("PK__ClientUs__57424D02A70A9BC6");

            entity.Property(e => e.ClientUserAuditDate).HasDefaultValueSql("(getdate())");

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
            entity.HasKey(e => e.DataDictionaryId).HasName("PK__tmp_ms_x__515DCA02C8AFA9DE");

            entity.HasOne(d => d.DataDictionaryGroup).WithMany(p => p.DataDictionaries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataDictionary_DataDictionarGroup");
        });

        modelBuilder.Entity<DataDictionaryAudit>(entity =>
        {
            entity.HasKey(e => e.DataDictionaryAuditId).HasName("PK__DataDict__A809F12022AB93CC");

            entity.Property(e => e.DataDictionaryAuditDate).HasDefaultValueSql("(getdate())");

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

        modelBuilder.Entity<DataDictionaryGroup>(entity =>
        {
            entity.HasKey(e => e.DataDictionaryGroupId).HasName("PK__DataDict__3640F87C838A402E");
        });

        modelBuilder.Entity<DataDictionaryGroupAudit>(entity =>
        {
            entity.HasKey(e => e.DataDictionaryGroupAuditId).HasName("PK__DataDict__68B06ADF91CA1E88");

            entity.Property(e => e.DataDictionaryGroupAuditDate).HasDefaultValueSql("(getdate())");

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
            entity.HasKey(e => e.UserId).HasName("PK__tmp_ms_x__1788CC4C1DFCBA3C");

            entity.Property(e => e.UserGuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.UserPasswordAttemptCount).HasDefaultValue(-1);

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_DataDictionary");
        });

        modelBuilder.Entity<UserAudit>(entity =>
        {
            entity.HasKey(e => e.UserAuditId).HasName("PK__UserAudi__CDD782FDF054306A");

            entity.Property(e => e.UserAuditDate).HasDefaultValueSql("(getdate())");

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
            entity.HasKey(e => e.WorkItemId).HasName("PK__tmp_ms_x__A10D1B45A8690677");

            entity.Property(e => e.WorkItemGuid).HasDefaultValueSql("(newid())");

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
            entity.HasKey(e => e.WorkItemAuditId).HasName("PK__WorkItem__5BB9AE8E6A1A08CB");

            entity.Property(e => e.WorkItemAuditDate).HasDefaultValueSql("(getdate())");

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

        modelBuilder.Entity<WorkItemView>(entity =>
        {
            entity.ToView("WorkItemView");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
