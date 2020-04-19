drop table dbo.TerminalGrouping;
go

drop table dbo.Capacities;
go

drop table dbo.DateCapacities;
go

create table dbo.Depot(
    id tinyint,
    name varchar(100),
    constraint PK_Depot primary key(id)
)
go

create table dbo.DepotCapacities (
    depot_id tinyint not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    day varchar(3) not null check (day in ('MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN')),
    load integer,
    deliveries decimal(4, 2),
    shifts tinyint,
    capacity decimal(10, 2),
    constraint PK_DepotCapacities primary key(depot_id, shift, day)
    constraint FK_DepotCapacities_Depot foreign key(id) references dbo.Depot(id),
)
go

create table dbo.DepotDateCapacities (
    depot_id tinyint not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    [date] date,
    load integer,
    deliveries decimal(4, 2),
    shifts tinyint,
    capacity decimal(10, 2),
    constraint PK_DepotDateCapacity primary key(depot_id, shift, [date]),
    constraint FK_DepotCapacities_Depot foreign key(id) references dbo.Depot(id),
)
go

alter table dbo.Terminals add id tinyint;
alter table dbo.Terminals add depot_id tinyint;
alter table dbo.Terminals drop constraint PK_Terminals;
alter table dbo.Terminals add constraint PK_Terminal primary key id;
alter table dbo.Terminals add constraint FK_Depot_Terminal foreign key depot_id references dbo.Depot(id);

