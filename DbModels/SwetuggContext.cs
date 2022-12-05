using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace swetugg_public.DbModels;

public partial class SwetuggContext : DbContext
{
    public SwetuggContext()
    {
    }

    public SwetuggContext(DbContextOptions<SwetuggContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conference> Conferences { get; set; }

    public virtual DbSet<ImageType> ImageTypes { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomSlot> RoomSlots { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionType> SessionTypes { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Speaker> Speakers { get; set; }

    public virtual DbSet<SpeakerImage> SpeakerImages { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<SponsorImage> SponsorImages { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Swetugg");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Finnish_Swedish_CI_AS");

        modelBuilder.Entity<Conference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Conferences");

            entity.Property(e => e.CfpEnd).HasColumnType("datetime");
            entity.Property(e => e.CfpStart).HasColumnType("datetime");
            entity.Property(e => e.CfpVipCode).HasMaxLength(50);
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.Start).HasColumnType("datetime");
        });

        modelBuilder.Entity<ImageType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ImageTypes");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);

            entity.HasOne(d => d.Conference).WithMany(p => p.ImageTypes)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.ImageTypes_dbo.Conferences_ConferenceId");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Rooms");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);

            entity.HasOne(d => d.Conference).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Rooms_dbo.Conferences_ConferenceId");
        });

        modelBuilder.Entity<RoomSlot>(entity =>
        {
            entity.HasKey(e => new { e.RoomId, e.SlotId }).HasName("PK_dbo.RoomSlots");

            entity.HasIndex(e => e.AssignedSessionId, "IX_AssignedSessionId");

            entity.HasIndex(e => e.RoomId, "IX_RoomId");

            entity.HasIndex(e => e.SlotId, "IX_SlotId");

            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.Start).HasColumnType("datetime");

            entity.HasOne(d => d.AssignedSession).WithMany(p => p.RoomSlots)
                .HasForeignKey(d => d.AssignedSessionId)
                .HasConstraintName("FK_dbo.RoomSlots_dbo.Sessions_AssignedSessionId");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomSlots)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.RoomSlots_dbo.Rooms_RoomId");

            entity.HasOne(d => d.Slot).WithMany(p => p.RoomSlots)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.RoomSlots_dbo.Slots_SlotId");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Sessions");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.HasIndex(e => e.SessionTypeId, "IX_SessionTypeId");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.VideoUrl).HasMaxLength(500);

            entity.HasOne(d => d.Conference).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Sessions_dbo.Conferences_ConferenceId");

            entity.HasOne(d => d.SessionType).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.SessionTypeId)
                .HasConstraintName("FK_dbo.Sessions_dbo.SessionTypes_SessionTypeId");

            entity.HasMany(d => d.Speakers).WithMany(p => p.Sessions)
                .UsingEntity<Dictionary<string, object>>(
                    "SessionSpeakers",
                    r => r.HasOne<Speaker>().WithMany()
                        .HasForeignKey("SpeakerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_dbo.SessionSpeakers_dbo.Speakers_SpeakerId"),
                    l => l.HasOne<Session>().WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_dbo.SessionSpeakers_dbo.Sessions_SessionId"),
                    j =>
                    {
                        j.HasKey("SessionId", "SpeakerId").HasName("PK_dbo.SessionSpeakers");
                        j.HasIndex(new[] { "SessionId" }, "IX_SessionId");
                        j.HasIndex(new[] { "SpeakerId" }, "IX_SpeakerId");
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.Sessions)
                .UsingEntity<Dictionary<string, object>>(
                    "SessionTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_dbo.SessionTags_dbo.Tags_Tag_Id"),
                    l => l.HasOne<Session>().WithMany()
                        .HasForeignKey("SessionId")
                        .HasConstraintName("FK_dbo.SessionTags_dbo.Sessions_Session_Id"),
                    j =>
                    {
                        j.HasKey("SessionId", "TagId").HasName("PK_dbo.SessionTags");
                        j.HasIndex(new[] { "SessionId" }, "IX_Session_Id");
                        j.HasIndex(new[] { "TagId" }, "IX_Tag_Id");
                    });
        });

        modelBuilder.Entity<SessionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SessionTypes");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);

            entity.HasOne(d => d.Conference).WithMany(p => p.SessionTypes)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SessionTypes_dbo.Conferences_ConferenceId");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Slots");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.Start).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.Conference).WithMany(p => p.Slots)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Slots_dbo.Conferences_ConferenceId");
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Speakers");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.Property(e => e.Bio).HasColumnType("ntext");
            entity.Property(e => e.Company).HasMaxLength(250);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.GitHub).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.SessionizeImageUrl).HasMaxLength(1000);
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.Twitter).HasMaxLength(50);
            entity.Property(e => e.Web).HasMaxLength(250);

            entity.HasOne(d => d.Conference).WithMany(p => p.Speakers)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Speakers_dbo.Conferences_ConferenceId");

            entity.HasMany(d => d.Tags).WithMany(p => p.Speakers)
                .UsingEntity<Dictionary<string, object>>(
                    "SpeakerTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_dbo.SpeakerTags_dbo.Tags_Tag_Id"),
                    l => l.HasOne<Speaker>().WithMany()
                        .HasForeignKey("SpeakerId")
                        .HasConstraintName("FK_dbo.SpeakerTags_dbo.Speakers_Speaker_Id"),
                    j =>
                    {
                        j.HasKey("SpeakerId", "TagId").HasName("PK_dbo.SpeakerTags");
                        j.HasIndex(new[] { "SpeakerId" }, "IX_Speaker_Id");
                        j.HasIndex(new[] { "TagId" }, "IX_Tag_Id");
                    });
        });

        modelBuilder.Entity<SpeakerImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SpeakerImages");

            entity.HasIndex(e => e.ImageTypeId, "IX_ImageTypeId");

            entity.HasIndex(e => e.SpeakerId, "IX_SpeakerId");

            entity.Property(e => e.ImageUrl).HasMaxLength(1000);

            entity.HasOne(d => d.ImageType).WithMany(p => p.SpeakerImages)
                .HasForeignKey(d => d.ImageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SpeakerImages_dbo.ImageTypes_ImageTypeId");

            entity.HasOne(d => d.Speaker).WithMany(p => p.SpeakerImages)
                .HasForeignKey(d => d.SpeakerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SpeakerImages_dbo.Speakers_SpeakerId");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Sponsors");

            entity.HasIndex(e => e.ConferenceId, "IX_ConferenceId");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.Twitter).HasMaxLength(50);
            entity.Property(e => e.Web).HasMaxLength(250);

            entity.HasOne(d => d.Conference).WithMany(p => p.Sponsors)
                .HasForeignKey(d => d.ConferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Sponsors_dbo.Conferences_ConferenceId");
        });

        modelBuilder.Entity<SponsorImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SponsorImages");

            entity.HasIndex(e => e.ImageTypeId, "IX_ImageTypeId");

            entity.HasIndex(e => e.SponsorId, "IX_SponsorId");

            entity.Property(e => e.ImageUrl).HasMaxLength(1000);

            entity.HasOne(d => d.ImageType).WithMany(p => p.SponsorImages)
                .HasForeignKey(d => d.ImageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SponsorImages_dbo.ImageTypes_ImageTypeId");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.SponsorImages)
                .HasForeignKey(d => d.SponsorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.SponsorImages_dbo.Sponsors_SponsorId");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Tags");

            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
