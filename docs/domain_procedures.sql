CREATE OR REPLACE PROCEDURE security.Domain_Create
(
    Name VARCHAR(150),
    UserRegistration INT
)
AS
$$
BEGIN
    INSERT INTO security.Domain(Name,UserRegistration)
    VALUES(Name,UserRegistration) RETURNING Id;

END;
$$ LANGUAGE plpgsql;