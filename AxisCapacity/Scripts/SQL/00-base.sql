drop table dbo.Capacity;

go

drop table dbo.TerminalGrouping;

go


create table dbo.Capacities (
    terminal varchar(100) not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    day varchar(3) not null check (day in ('MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT', 'SUN')),
    load integer,
    deliveries decimal(4, 2),
    shifts tinyint,
    capacity integer,
    constraint PK_TerminalCapacity primary key(terminal, shift, day)
)

go

create table dbo.TerminalGroups (
	terminal varchar(100) not null,
	group_id tinyint not null,
	constraint PK_TerminalGrouping primary key(terminal)
)

go

insert into dbo.TerminalGroups(terminal, group_id) values ('Dagenham', 1);
insert into dbo.TerminalGroups(terminal, group_id) values ('Grays', 1);

go

-- DAGENHAM
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'MON', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'TUE', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'WED', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'THU', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'FRI', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'SAT', 34498, 1.62, 5, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'AM', 'SUN', 34498, 1.62, 5, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'MON', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'TUE', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'WED', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'THU', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'FRI', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'SAT', 34498, 1.61, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Dagenham', 'PM', 'SUN', 34498, 1.61, 3, null);

go

-- GRAYS
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'MON', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'TUE', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'WED', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'THU', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'FRI', 34498, 1.62, 7, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'SAT', 34498, 1.62, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'AM', 'SUN', 34498, 1.62, 3, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'MON', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'TUE', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'WED', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'THU', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'FRI', 34498, 1.61, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'SAT', 34498, 1.61, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grays', 'PM', 'SUN', 34498, 1.61, 1, null);

go

-- WESTERLEIGH
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'MON', 34096, 2.34, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'TUE', 34096, 2.34, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'WED', 34096, 2.34, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'THU', 34096, 2.34, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'FRI', 34096, 2.34, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'SAT', 34096, 2.34, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'AM', 'SUN', 34096, 2.34, 0, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'MON', 34096, 2.32, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'TUE', 34096, 2.32, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'WED', 34096, 2.32, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'THU', 34096, 2.32, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'FRI', 34096, 2.32, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'SAT', 34096, 2.32, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Westerleigh', 'PM', 'SUN', 34096, 2.32, 0, null);

go

-- KINGSBURY
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'MON', 33862, 2.34, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'TUE', 33862, 2.34, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'WED', 33862, 2.34, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'THU', 33862, 2.34, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'FRI', 33862, 2.34, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'SAT', 33862, 2.34, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'AM', 'SUN', 33862, 2.34, 1, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'MON', 33862, 2.39, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'TUE', 33862, 2.39, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'WED', 33862, 2.39, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'THU', 33862, 2.39, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'FRI', 33862, 2.39, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'SAT', 33862, 2.39, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Kingsbury', 'PM', 'SUN', 33862, 2.39, 0, null);

go

-- IMMINGHAM
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'MON', 34043, 1.66, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'TUE', 34043, 1.66, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'WED', 34043, 1.66, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'THU', 34043, 1.66, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'FRI', 34043, 1.66, 4, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'SAT', 34043, 1.66, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'AM', 'SUN', 34043, 1.66, 1, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'MON', 34043, 1.65, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'TUE', 34043, 1.65, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'WED', 34043, 1.65, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'THU', 34043, 1.65, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'FRI', 34043, 1.65, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'SAT', 34043, 1.65, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Immingham', 'PM', 'SUN', 34043, 1.65, 0, null);

go


-- JARROW
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'MON', 33876, 1.72, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'TUE', 33876, 1.72, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'WED', 33876, 1.72, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'THU', 33876, 1.72, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'FRI', 33876, 1.72, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'SAT', 33876, 1.72, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'AM', 'SUN', 33876, 1.72, 0, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'MON', 33876, 1.83, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'TUE', 33876, 1.83, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'WED', 33876, 1.83, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'THU', 33876, 1.83, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'FRI', 33876, 1.83, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'SAT', 33876, 1.83, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Jarrow', 'PM', 'SUN', 33876, 1.83, 0, null);

go


-- GRANGEMOUTH
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'MON', 33013, 1.88, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'TUE', 33013, 1.88, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'WED', 33013, 1.88, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'THU', 33013, 1.88, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'FRI', 33013, 1.88, 2, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'SAT', 33013, 1.88, 1, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'AM', 'SUN', 33013, 1.88, 1, null);

insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'MON', 33013, 1.95, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'TUE', 33013, 1.95, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'WED', 33013, 1.95, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'THU', 33013, 1.95, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'FRI', 33013, 1.95, 3, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'SAT', 33013, 1.95, 0, null);
insert into dbo.Capacities (terminal, shift, day, load, deliveries, shifts, capacity) values ('Grangemouth', 'PM', 'SUN', 33013, 1.95, 1, null);

go

