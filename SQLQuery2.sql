CREATE TABLE [dbo].[Class] (
    [ClassId] [int] NOT NULL IDENTITY,
    [CountPeople] [int] NOT NULL,
    [Reiting] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Class] PRIMARY KEY ([ClassId])
)
CREATE TABLE [dbo].[Shedule] (
    [SheduleId] [int] NOT NULL IDENTITY,
    [TeacherId] [int],
    [ClassId] [int],
    [NumberObject] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_dbo.Shedule] PRIMARY KEY ([SheduleId])
)
CREATE INDEX [IX_TeacherId] ON [dbo].[Shedule]([TeacherId])
CREATE INDEX [IX_ClassId] ON [dbo].[Shedule]([ClassId])
CREATE TABLE [dbo].[Teacher] (
    [Id] [int] NOT NULL IDENTITY,
    [FIOteacher] [nvarchar](100) NOT NULL,
    [Birthday] [datetime] NOT NULL,
    [Exp] [int] NOT NULL,
    [CountHour] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Teacher] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Student] (
    [Id] [int] NOT NULL IDENTITY,
    [FIO] [nvarchar](100) NOT NULL,
    [Birtday] [datetime] NOT NULL,
    [ClassId] [int],
    CONSTRAINT [PK_dbo.Student] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ClassId] ON [dbo].[Student]([ClassId])
ALTER TABLE [dbo].[Shedule] ADD CONSTRAINT [FK_dbo.Shedule_dbo.Class_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Class] ([ClassId])
ALTER TABLE [dbo].[Shedule] ADD CONSTRAINT [FK_dbo.Shedule_dbo.Teacher_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teacher] ([Id])
ALTER TABLE [dbo].[Student] ADD CONSTRAINT [FK_dbo.Student_dbo.Class_ClassId] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Class] ([ClassId])
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](150) NOT NULL,
    [ContextKey] [nvarchar](300) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
)

