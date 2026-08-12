CREATE TABLE "User" (
	"Id"	TEXT,
	"CreatedTicks"	integer,
	"UpdatedTicks"	integer,
	"Name"	TEXT,
	"Description"	TEXT,
	"Email"	TEXT,
	"PhoneNumber"	TEXT,
	"UserType"	integer,
	"IsEmailConfirm"	integer,
	"IsDelete"	integer,
	"Password"	TEXT,
	"RememberMe"	integer,
	"UserUpdatedId"	TEXT,
	"UserCreatedId"	TEXT,
	PRIMARY KEY("Id")
)