IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee')
BEGIN
Create Table dbo.Employee (
	Id Int Not NULL Identity(1,1) Primary Key,
	[Name] Nvarchar(100) Not NULL

)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'IdCard')
BEGIN
Create Table dbo.IdCard (
	Number Int Not NULL Primary Key,
	[Description] Nvarchar(100) NULL
)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Division')
BEGIN
Create Table dbo.Division (
	Code Nvarchar(50) Not NULL Primary Key,
	[Name] Nvarchar(100) NULL
)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Task')
BEGIN
Create Table dbo.Task (
	Code Nvarchar(50) Not NULL Primary Key,
	[Name] Nvarchar(100) NULL
)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmployeeCard')
BEGIN
Create Table dbo.EmployeeCard (
	Id Int Not NULL Identity(1,1) Primary Key,
	EmployeeId Int Not Null,
	CardNumber Int Not Null,
	FOREIGN KEY (EmployeeId) REFERENCES Employee(Id),
	FOREIGN KEY (CardNumber) REFERENCES IdCard(Number),
)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DivisionMember')
BEGIN
Create Table dbo.DivisionMember (
	Id Int Not NULL Identity(1,1) Primary Key,
	DivisionCode Nvarchar(50) Not Null,
	EmployeeId Int Not Null,
	FOREIGN KEY (DivisionCode) REFERENCES Division(Code),
	FOREIGN KEY (EmployeeId) REFERENCES Employee(Id),
)
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'EmployeeTask')
BEGIN
Create Table dbo.EmployeeTask (
	Id Int Not NULL Identity(1,1) Primary Key,
	EmployeeId Int Not Null,
	TaskCode Nvarchar(50) Not Null,
	FOREIGN KEY (EmployeeId) REFERENCES Employee(Id),
	FOREIGN KEY (TaskCode) REFERENCES Task(Code),
)
END


