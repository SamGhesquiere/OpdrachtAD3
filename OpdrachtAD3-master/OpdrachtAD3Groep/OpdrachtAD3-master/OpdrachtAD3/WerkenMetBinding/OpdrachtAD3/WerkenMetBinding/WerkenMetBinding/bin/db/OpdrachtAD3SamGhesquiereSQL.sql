Use dbTest;

go

if exists(select table_name from INFORMATION_SCHEMA.TABLES where table_name = 'tblAppointments')
	drop table tblAppointments

go
if exists(select table_name from INFORMATION_SCHEMA.TABLES where table_name = 'tblAgendas')
	drop table tblAgendas


go
CREATE TABLE tblAgendas (
    agendaId int not null IDENTITY primary key,
    agendaName varchar(50)
     
);
go
CREATE TABLE tblAppointments (
	appointmentId int not null IDENTITY(1,1) primary key,
    appointmentName varchar(50),
	dateStart DateTime not null,
	dateEnd DateTime2,
	appointmentDescription Varchar(255),
	idFromAgenda int not null foreign key references tblAgendas(agendaId)
    
);
go 
	
--INSERT INTO tblAgendas(agendaName)
--VALUES ('xD')
go
/*

SELECT * FROM tblAgendas;
SELECT * FROM tblAppointments;


*/
go
/*EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' 
GO

EXEC sp_MSForEachTable 'DELETE FROM ?' 
GO*/