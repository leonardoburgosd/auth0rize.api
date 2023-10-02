INSERT INTO domain.Application([Key],[Name],[RegistrationUser])
VALUES('088282C5A834543D60D8635AA16113A8F647A2A52B753D3A4F1D24A8C94F6918','Intranet','Leonardo Burgos Díaz')
GO
INSERT INTO [identity].[TypeUser](Detail)
                        VALUES('Superadmin')
GO

select * from [identity].[User]


INSERT INTO domain.Application([Key],[Name],[RegistrationUser])
VALUES('CBB82EBEC6051B780AD26655EE48B5B42658C4AC49438888AAC829E82195584F','Auth','Leonardo Burgos Díaz')

select * from domain.Application
go
select * from domain.ApplicationUser
go

INSERT INTO domain.ApplicationUser(ApplicationId,UserId,RegistrationUser)
VALUES(2,1,'Leonardo Burgos Díaz')