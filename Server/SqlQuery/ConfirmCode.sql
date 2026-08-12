CREATE TABLE "ConfirmCode" (
	"Id"	INTEGER,
	"CreatedTicks"	integer,
	"UpdatedTicks"	integer,
	"UserId"	TEXT,
	"Code"	integer,
	"ExpireDate"	integer,
	PRIMARY KEY("Id" AUTOINCREMENT)
)