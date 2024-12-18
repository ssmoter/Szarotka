CREATE TABLE "RegisterConfirmEmailUser" (
"Id" INTEGER PRIMARY KEY AUTOINCREMENT,
"CreatedTicks" integer ,
"UpdatedTicks" integer ,
"UserId" varchar(36) ,
"Code" integer ,
"ExpireDate" integer 
)