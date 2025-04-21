CREATE TABLE "ConfirmCode" (
	"Id"	INTEGER,
	"CreatedTicks"	integer,
	"UpdatedTicks"	integer,
	"UserId"	varchar(36),
	"Code"	integer,
	"ExpireDate"	integer,
	PRIMARY KEY("Id" AUTOINCREMENT)
)