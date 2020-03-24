create table dbo.Capacity (
    terminal varchar(100) not null,
    shift varchar(2) not null check (shift in ('AM', 'PM')),
    avg_load integer,
    avg_deliveries  decimal(4, 2),
    mon tinyint,
    tue tinyint,
    wed tinyint,
    thu tinyint,
    fri tinyint,
    sat tinyint,
    sun tinyint,
    constraint PK_TerminalCapacity primary key(terminal, shift),
    constraint FK_TerminalCapacity_Terminal foreign key(terminal) references dbo.Terminals(name)    
)

create table dbo.TerminalGrouping (
	terminal varchar(100) not null,
	group_id tinyint not null,
	constraint PK_TerminalGrouping primary key(terminal),
	constraint FK_TerminalGrouping_Terminal foreign key(terminal) references dbo.Terminals(name)
)