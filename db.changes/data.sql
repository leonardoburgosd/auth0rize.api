INSERT INTO domain.Application([Key],[Name],[RegistrationUser])
VALUES('088282C5A834543D60D8635AA16113A8F647A2A52B753D3A4F1D24A8C94F6918','Intranet','Leonardo Burgos Díaz')
GO
INSERT INTO [identity].[TypeUser](Detail)
                        VALUES('Superadmin')
GO

select * from [identity].[User]


