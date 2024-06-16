CREATE SCHEMA security;
CREATE TABLE security.Type
(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,

    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT,
    DateUpdate TIMESTAMP,
    UserUpdate INT,
    DateDeleted TIMESTAMP,
    UserDeleted INT,
    IsDeleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE security.Domain
(
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,

    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT,
    DateUpdate TIMESTAMP,
    UserUpdate INT,
    DateDeleted TIMESTAMP,
    UserDeleted INT,
    IsDeleted BOOLEAN DEFAULT FALSE
);
CREATE TABLE security.User
(
    Id SERIAL PRIMARY KEY ,
    Name VARCHAR(150) NOT NULL,
    LastName VARCHAR(150) NOT NULL,
    MotherLastName VARCHAR(150) NOT NULL,
    UserName VARCHAR(30) NOT NULL UNIQUE,
    Email VARCHAR(30) NOT NULL UNIQUE,
    Password BYTEA NOT NULL,
    Salt BYTEA NOT NULL,
    IsDoubleFactorActivate BOOLEAN DEFAULT FALSE,
    Avatar VARCHAR(100) NOT NULL,
    Type INT REFERENCES security.Type(Id),
    Domain INT REFERENCES security.Domain(Id),

    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT,
    DateUpdate TIMESTAMP,
    UserUpdate INT,
    DateDeleted TIMESTAMP,
    UserDeleted INT,
    IsDeleted BOOLEAN DEFAULT FALSE
);
IF NOT EXISTS "uuid-ossp";
CREATE SCHEMA organization;
CREATE TABLE organization.Application
(
    Id SERIAL PRIMARY KEY ,
    Name VARCHAR(150) NOT NULL,
    Code UUID DEFAULT uuid_generate_v4(),
    Description VARCHAR(150),

    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT,
    DateUpdate TIMESTAMP,
    UserUpdate INT,
    DateDeleted TIMESTAMP,
    UserDeleted INT,
    IsDeleted BOOLEAN DEFAULT FALSE
);
CREATE TABLE organization.ApplicationDomain
(
    Application INT REFERENCES organization.Application(Id),
    Domain INT REFERENCES security.Domain(Id),

    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UserRegistration INT,
    DateUpdate TIMESTAMP,
    UserUpdate INT,
    DateDeleted TIMESTAMP,
    UserDeleted INT,
    IsDeleted BOOLEAN  DEFAULT FALSE
);