create table dbo.DateCapacities (
    terminal varchar(100) not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    [date] date,
    load integer,
    deliveries decimal(4, 2),
    shifts tinyint,
    capacity decimal(10, 2),
    constraint PK_TerminalDateCapacity primary key(terminal, shift, [date])
)