namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsersAndManageMoviesRole : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'57ab519a-e948-4e0a-a0b3-6f05c3089919', N'CanManageMovies')
                INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'18ffd659-4edc-4644-8542-69d265f7be63', N'bitxwise@gmail.com', 0, N'AIzsatyAnaLtxIum/Dg3icSlX9E96uVy+us+jIZP8lmKtZpz2zC5TPBQ6MonMZKa0A==', N'4b956bd4-e086-4a4c-a3cc-8bd3511c5f6b', NULL, 0, 0, NULL, 1, 0, N'bitxwise@gmail.com')
                INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'41e8256a-9cb5-4d3e-a496-98bb31bbe694', N'admin@vidly.com', 0, N'AHtYc+r5u0eTUKA8W5JuQkx2OLJkbJFKb4UjZiXbslxvO4DxBbjAZvs2f/s0tUSEOQ==', N'52067c66-8eed-4ab2-82e7-080982df3b5b', NULL, 0, 0, NULL, 1, 0, N'admin@vidly.com')
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'41e8256a-9cb5-4d3e-a496-98bb31bbe694', N'57ab519a-e948-4e0a-a0b3-6f05c3089919')
            ");
        }
        
        public override void Down()
        {
        }
    }
}
