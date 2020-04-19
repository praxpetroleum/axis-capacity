drop table dbo.Capacities;
go

drop table dbo.DateCapacities;
go


create table dbo.Depot(
    id int identity(1,1),
    name varchar(256) not null,
    constraint PK_Depot primary key(id)
)
go

create table dbo.DepotCapacities (
    depot_id int not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    day varchar(3) not null check (day in ('MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN')),
    load integer,
    deliveries decimal(4, 2),
    shifts int,
    capacity decimal(10, 2),
    constraint PK_DepotCapacities primary key(depot_id, shift, day),
    constraint FK_DepotCapacities_Depot foreign key(depot_id) references dbo.Depot(id),
)
go

create table dbo.DepotDateCapacities (
    depot_id int not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    [date] date,
    load integer,
    deliveries decimal(4, 2),
    shifts int,
    capacity decimal(10, 2),
    constraint PK_DepotDateCapacity primary key(depot_id, shift, [date]),
    constraint FK_DepotDateCapacities_Depot foreign key(depot_id) references dbo.Depot(id),
)
go

alter table dbo.Terminals add depot_id int;
alter table dbo.Terminals add constraint FK_Depot_Terminal foreign key(depot_id) references dbo.Depot(id);

