CREATE OR REPLACE PROCEDURE security.User_Create
(
    IN Name VARCHAR(150),
    IN LastName VARCHAR(150),
    IN MotherLastName VARCHAR(150),
    IN UserName VARCHAR(30),
    IN Email VARCHAR(30),
    IN Password BYTEA,
    IN Salt BYTEA,
    IN Avatar VARCHAR(100),
    IN Type INT,
    IN Domain INT,
    IN UserRegistration INT,
    IN IsDoubleFactorActivate BOOL
) 
AS
$$
BEGIN
    INSERT INTO security.User
        (Name, LastName, MotherLastName, UserName, Email, Password,
        Salt, IsDoubleFactorActivate, Avatar, Type, Domain, UserRegistration)
    VALUES(Name, LastName, MotherLastName, UserName, Email, Password,
            Salt, IsDoubleFactorActivate, Avatar, Type, Domain, UserRegistration)
    RETURNING Id;
END;
$$ LANGUAGE plpgsql;

CALL security.User_Create ('John', 'Doe', 'Johnson', 'johndoe', 'johndoe@example.com', E'\\xDEADBEEF'::BYTEA, E'\\xCAFEBABE'::BYTEA, 'avatar.jpg', 1, 1, 1, TRUE)

CREATE OR REPLACE PROCEDURE security.User_Get
(
    UserName VARCHAR
(30)
)
AS
$$
BEGIN
    SELECT *
    FROM security.User u
    INNER JOIN security.Type t ON u.type = t.id
    INNER JOIN security.Domain d ON u.domain = d.id
    WHERE UserName = UserName;
END;
$$ LANGUAGE plpgsql;
