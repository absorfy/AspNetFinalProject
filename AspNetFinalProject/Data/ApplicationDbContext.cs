using AspNetFinalProject.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AspNetFinalProject.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<PersonalInfo> PersonalInfos { get; set; } = null!;
    public DbSet<UserActionLog> UserActionLogs { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<ActivityTrackingSettings> ActivityTrackingSettings { get; set; } = null!;
    public DbSet<WorkSpace> WorkSpaces { get; set; } = null!;
    public DbSet<WorkSpaceParticipant> WorkSpaceParticipants { get; set; } = null!;
    public DbSet<Board> Boards { get; set; } = null!;
    public DbSet<BoardParticipant> BoardParticipants { get; set; } = null!;
    public DbSet<BoardList> Lists { get; set; } = null!;
    public DbSet<Card> Cards { get; set; } = null!;
    public DbSet<CardParticipant> CardParticipants { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<TagCard> TagCards { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<CardAttachment> CardAttachments { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserProfile>()
            .HasOne(u => u.IdentityUser)
            .WithOne()
            .HasForeignKey<UserProfile>(u => u.IdentityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Subscription>()
            .HasKey(s => new { s.UserProfileId, s.EntityName, s.EntityId });
        
        modelBuilder.Entity<WorkSpaceParticipant>()
            .HasKey(x => new { x.WorkSpaceId, x.UserProfileId });

        modelBuilder.Entity<BoardParticipant>()
            .HasKey(x => new { x.BoardId, x.UserProfileId });
        
        modelBuilder.Entity<CardParticipant>()
            .HasKey(cp => new { cp.CardId, cp.UserProfileId });
        
        modelBuilder.Entity<TagCard>()
            .HasKey(tc => new { tc.TagId, tc.CardId });
        
        modelBuilder.Entity<WorkSpace>()
            .HasOne(w => w.Author)
            .WithMany(u => u.CreatedWorkspaces)
            .HasForeignKey(w => w.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<WorkSpace>()
            .HasOne(w => w.DeletedByUser)
            .WithMany(u => u.DeletedWorkspaces)
            .HasForeignKey(w => w.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Board>()
            .HasOne(b => b.Author)
            .WithMany(u => u.CreatedBoards)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Board>()
            .HasOne(b => b.DeletedByUser)
            .WithMany(u => u.DeletedBoards)
            .HasForeignKey(b => b.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<BoardList>()
            .HasOne(l => l.Author)
            .WithMany(u => u.CreatedLists)
            .HasForeignKey(l => l.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<BoardList>()
            .HasOne(l => l.DeletedByUser)
            .WithMany(u => u.DeletedLists)
            .HasForeignKey(l => l.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UserActionLog>()
            .HasOne(x => x.UserProfile)
            .WithMany(u => u.ActionLogs)
            .HasForeignKey(x => x.UserProfileId);
        
        modelBuilder.Entity<Card>()
            .HasOne(c => c.Author)
            .WithMany(u => u.CreatedCards)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Card>()
            .HasOne(c => c.DeletedByUser)
            .WithMany(u => u.DeletedCards)
            .HasForeignKey(c => c.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.CreatedComments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.DeletedByUser)
            .WithMany(u => u.DeletedComments)
            .HasForeignKey(c => c.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<CardAttachment>()
            .HasOne(a => a.Author)
            .WithMany(u => u.UploadedAttachments)
            .HasForeignKey(a => a.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}