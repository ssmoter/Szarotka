CREATE TABLE "User" (
"Id" varchar(36) PRIMARY KEY ,
"CreatedTicks" integer ,
"UpdatedTicks" integer ,
"Name" varchar(36) ,
"Description" varchar(36) ,
"Email" varchar(36) ,
"PhoneNumber" varchar(36) ,
"UserType"  integer ,
"IsEmailConfirm" integer,
"IsDelete" integer,
"Password" varchar(36)
)