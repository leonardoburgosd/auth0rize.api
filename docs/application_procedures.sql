CREATE OR REPLACE PROCEDURE organization.Application_Get
(UserId INT) 
AS
$$
BEGIN
    SELECT *
    FROM organization.applicationdomain
    WHERE IsDeleted = FALSE AND domain = (
    SELECT domain
        FROM security.user
        WHERE isdeleted = FALSE AND id = UserId
    LIMIT 1
);
END;
$$ LANGUAGE plpgsql;
CREATE OR REPLACE PROCEDURE organization.Application_Create
(
    Name VARCHAR(150),
    Description VARCHAR(150),
    UserRegistration INT
)
AS
$$
BEGIN
    INSERT INTO organization.Application(Name,Description,UserRegistration)
    VALUES(Name,Description,UserRegistration) RETURNING Id;
END;
$$ LANGUAGE plpgsql;