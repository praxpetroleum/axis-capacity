/*
##########################################################################<br>
-- Name:                dev-212-add-date-capacities.sql
-- Date:                (21/04/2020) 
-- Author:              Evans Thereska (evans.thereska@prax.com )
-- Company:             Prax Petroleum Ltd
-- Purpose:             Capacities now belong to a Depot which can have service multiple Terminals.
                        Also added ability to query capacities by month.
-- Ticket ID:           https://praxpetroleum.atlassian.net/browse/DEV-212
-- Servers/DB’s:        SOWEYDB02/Axis
-- Usage:               
-- Impact:              Operations teams can now upload capacities based on Depot
-- Required grants:     
-- Called by:           Run once by DBA 
-- NOTE:                

##########################################################################<br>
-- ver:                                  user    date        change
-- 1.0	                         evans.thereska	20200421	initial   
##########################################################################<br>
*/

use Axis;
go

-- DDL

-- Drop old tables
drop table dbo.Capacities;
go
drop table dbo.DateCapacities;
go


-- Create new tables
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
    load int,
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
    load int,
    deliveries decimal(4, 2),
    shifts int,
    capacity decimal(10, 2),
    constraint PK_DepotDateCapacity primary key(depot_id, shift, [date]),
    constraint FK_DepotDateCapacities_Depot foreign key(depot_id) references dbo.Depot(id),
)
go

-- Table terminals now has a depot reference
alter table dbo.Terminals add depot_id int;
alter table dbo.Terminals add constraint FK_Depot_Terminal foreign key(depot_id) references dbo.Depot(id);



-- DML

-- Depots available
insert into dbo.Depot(name) values ('Thurrock');
insert into dbo.Depot(name) values ('Westerleigh');
insert into dbo.Depot(name) values ('Kingsbury');
insert into dbo.Depot(name) values ('Immingham');
insert into dbo.Depot(name) values ('Jarrow');
insert into dbo.Depot(name) values ('Grangemouth');
go

-- Terminal depot mapping
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Westerleigh') where name = 'Westerleigh';
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Thurrock') where name = 'Grays';
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Thurrock') where name = 'Dagenham';
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Kingsbury') where name = 'Kingsbury';
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Immingham') where name = 'Immingham';
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Jarrow') where name = 'Jarrow';
update dbo.Terminals set depot_id = (select id from dbo.Depot where name = 'Grangemouth') where name = 'Grangemouth';
go


-- Depot capacity mappings

-- Thurrock
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'MON', 34498, 1.62, 14, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'TUE', 34498, 1.62, 14, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'WED', 34498, 1.62, 14, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'THU', 34498, 1.62, 14, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'FRI', 34498, 1.62, 14, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'SAT', 34498, 1.62, 9, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'AM', 'SUN', 34498, 1.62, 8, null);

insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'MON', 34498, 1.61, 6, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'TUE', 34498, 1.61, 6, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'WED', 34498, 1.61, 6, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'THU', 34498, 1.61, 6, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'FRI', 34498, 1.61, 6, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'SAT', 34498, 1.61, 6, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Thurrock'), 'PM', 'SUN', 34498, 1.61, 4, null);

go

-- Westerleigh
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'MON', 34096, 2.34, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'TUE', 34096, 2.34, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'WED', 34096, 2.34, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'THU', 34096, 2.34, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'FRI', 34096, 2.34, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'SAT', 34096, 2.34, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'AM', 'SUN', 34096, 2.34, 0, null);

insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'MON', 34096, 2.32, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'TUE', 34096, 2.32, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'WED', 34096, 2.32, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'THU', 34096, 2.32, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'FRI', 34096, 2.32, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'SAT', 34096, 2.32, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Westerleigh'), 'PM', 'SUN', 34096, 2.32, 0, null);

go

-- KINGSBURY
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'MON', 33862, 2.34, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'TUE', 33862, 2.34, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'WED', 33862, 2.34, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'THU', 33862, 2.34, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'FRI', 33862, 2.34, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'SAT', 33862, 2.34, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'AM', 'SUN', 33862, 2.34, 1, null);

insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'MON', 33862, 2.39, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'TUE', 33862, 2.39, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'WED', 33862, 2.39, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'THU', 33862, 2.39, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'FRI', 33862, 2.39, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'SAT', 33862, 2.39, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Kingsbury'), 'PM', 'SUN', 33862, 2.39, 0, null);

go

-- IMMINGHAM
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'MON', 34043, 1.66, 4, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'TUE', 34043, 1.66, 4, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'WED', 34043, 1.66, 4, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'THU', 34043, 1.66, 4, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'FRI', 34043, 1.66, 4, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'SAT', 34043, 1.66, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'AM', 'SUN', 34043, 1.66, 1, null);

insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'MON', 34043, 1.65, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'TUE', 34043, 1.65, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'WED', 34043, 1.65, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'THU', 34043, 1.65, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'FRI', 34043, 1.65, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'SAT', 34043, 1.65, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Immingham'), 'PM', 'SUN', 34043, 1.65, 0, null);

go


-- JARROW
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'MON', 33876, 1.72, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'TUE', 33876, 1.72, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'WED', 33876, 1.72, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'THU', 33876, 1.72, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'FRI', 33876, 1.72, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'SAT', 33876, 1.72, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'AM', 'SUN', 33876, 1.72, 0, null);

insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'MON', 33876, 1.83, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'TUE', 33876, 1.83, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'WED', 33876, 1.83, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'THU', 33876, 1.83, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'FRI', 33876, 1.83, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'SAT', 33876, 1.83, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Jarrow'), 'PM', 'SUN', 33876, 1.83, 0, null);

go


-- GRANGEMOUTH
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'MON', 33013, 1.88, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'TUE', 33013, 1.88, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'WED', 33013, 1.88, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'THU', 33013, 1.88, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'FRI', 33013, 1.88, 2, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'SAT', 33013, 1.88, 1, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'AM', 'SUN', 33013, 1.88, 1, null);

insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'MON', 33013, 1.95, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'TUE', 33013, 1.95, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'WED', 33013, 1.95, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'THU', 33013, 1.95, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'FRI', 33013, 1.95, 3, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'SAT', 33013, 1.95, 0, null);
insert into dbo.DepotCapacities (depot_id, shift, day, load, deliveries, shifts, capacity) values ((select id from dbo.Depot where name = 'Grangemouth'), 'PM', 'SUN', 33013, 1.95, 1, null);

go


