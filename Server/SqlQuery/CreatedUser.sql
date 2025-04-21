CREATE TABLE "User" (
	"Id"	varchar(36),
	"CreatedTicks"	integer,
	"UpdatedTicks"	integer,
	"Name"	varchar(36),
	"Description"	varchar(36),
	"Email"	varchar(36),
	"PhoneNumber"	varchar(36),
	"UserType"	integer,
	"IsEmailConfirm"	integer,
	"IsDelete"	integer,
	"Password"	varchar(36),
	"RememberMe"	integer,
	"UserUpdatedId"	varchar(36),
	"UserCreatedId"	varchar(36),
	PRIMARY KEY("Id")
)