using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using WebApiPushNotifications.Shared;

namespace WebApiPushNotifications.Store.SQLServer
{
    internal class SqlServerDBContext : DbContext
    {
        public DbSet<PushSubscriptionUser> PushSubscriptionUser { get; set; }
        public DbSet<PushNotificationUser> PushNotificationUser { get; set; }
        public DbSet<PushSubscriptionTopic> PushSubscriptionTopic { get; set; }
        public DbSet<PushNotificationMessageType> PushNotificationMessageType { get; set; }
        public DbSet<PushNotificationMessageActions> PushNotificationMessageActions { get; set; }
        public DbSet<PushNotificationMessagesTemplate> PushNotificationMessagesTemplate { get; set; }
        public DbSet<PushNotificationMessageUrgency> PushNotificationMessageUrgency { get; set; }

        public SqlServerDBContext(DbContextOptions<SqlServerDBContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<PushSubscriptionTopic> webpushTopicEntityTypeBuilder = modelBuilder.Entity<PushSubscriptionTopic>();
            webpushTopicEntityTypeBuilder.HasKey(p => p.TopicId);

            EntityTypeBuilder<PushSubscriptionUser> webpushSubscriptionEntityTypeBuilder = modelBuilder.Entity<PushSubscriptionUser>();
            webpushSubscriptionEntityTypeBuilder.HasKey(p => p.SubscriptionId);
            //webpushSubscriptionEntityTypeBuilder.Ignore(p => p.Keys);

            EntityTypeBuilder<PushNotificationUser> pushNotificationUserEntityTypeBuilder = modelBuilder.Entity<PushNotificationUser>();
            pushNotificationUserEntityTypeBuilder.HasKey(e => e.NotificationId);
            //pushNotificationUserEntityTypeBuilder.Ignore(p => p.UserId);

            EntityTypeBuilder<PushNotificationMessagesTemplate> webpushMessageEntityTypeBuilder = modelBuilder.Entity<PushNotificationMessagesTemplate>();
            webpushMessageEntityTypeBuilder.HasKey(p => p.MessageId);

            EntityTypeBuilder<PushNotificationMessageActions> webpushMessageActionsEntityTypeBuilder = modelBuilder.Entity<PushNotificationMessageActions>();
            webpushMessageActionsEntityTypeBuilder.HasKey(p => p.MessageActionId);

            EntityTypeBuilder<PushNotificationMessageType> webpushMessageTypeEntityTypeBuilder = modelBuilder.Entity<PushNotificationMessageType>();
            webpushMessageTypeEntityTypeBuilder.HasKey(p => p.MessageTypeId);

            EntityTypeBuilder<PushNotificationMessageUrgency> webpushMessageUrgencyEntityTypeBuilder = modelBuilder.Entity<PushNotificationMessageUrgency>();
            webpushMessageUrgencyEntityTypeBuilder.HasKey(p => p.UrgencyId);

            modelBuilder.Entity<PushNotificationMessagesTemplate>()
                .HasMany(o => o.MessageActions)
                .WithOne(o => o.Message);

            modelBuilder.Entity<PushNotificationMessageActions>()
                .HasOne(o => o.Message)
                .WithMany(o => o.MessageActions);

        }
    }
}
