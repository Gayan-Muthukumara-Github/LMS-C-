CREATE TABLE [dbo].[users] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [email]         VARCHAR (MAX) NOT NULL,
    [username]      VARCHAR (MAX) NOT NULL,
    [password]      VARCHAR (MAX) NOT NULL,
    [date_register] DATE          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[books] (
    [id]             INT           IDENTITY (1, 1) NOT NULL,
    [book_title]     VARCHAR (MAX) NULL,
    [author]         VARCHAR (MAX) NULL,
    [published_date] DATE          NULL,
    [status]         VARCHAR (MAX) NULL,
    [image]          VARCHAR (MAX) NULL,
    [date_insert]    DATE          NULL,
    [date_update]    DATE          NULL,
    [date_delete]    DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE [dbo].[issues] (
    [id]          INT           NOT NULL IDENTITY,
    [issue_id]    VARCHAR (MAX) NULL,
    [full_name]   VARCHAR (MAX) NULL,
    [contact]     VARCHAR (MAX) NULL,
    [email]       VARCHAR (MAX) NULL,
    [book_title]  VARCHAR (MAX) NULL,
    [author]      VARCHAR (MAX) NULL,
    [image]       VARCHAR (MAX) NULL,
    [status]      VARCHAR (MAX) NULL,
    [issue_date]  DATE          NULL,
    [return_date] DATE          NULL,
    [date_insert] DATE          NULL,
    [date_update] DATE          NULL,
    [date_delete] DATE          NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

