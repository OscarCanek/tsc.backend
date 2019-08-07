use master
go

create database tsc
go

use tsc
go

-- countries table
-- name of 75 characters because https://www.worldatlas.com/articles/what-is-the-longest-country-name-in-the-world.html
create table countries
(
    id uniqueidentifier not null,
    common_name varchar(75) not null,
    iso_name varchar(75) not null,
    alfa2 char(2) not null,
    alfa3 char(3) not null,
    country_code smallint not null,
    phone_prefix varchar(6) not null,
    row_version rowversion not null,
    constraint pk_countries primary key clustered(id)
)
go

-- default primary key value
alter table countries
    add constraint df_countries_id 
    default newsequentialid() for id
go

alter table countries
	add constraint ak_countries_alfa2
	unique (alfa2)
go

alter table countries
	add constraint ak_countries_alfa3
	unique (alfa3)
go

alter table countries
	add constraint ak_countries_country_code
	unique (country_code)
go

-- subdivisions table
create table subdivisions
(
    id uniqueidentifier not null,
    country_id uniqueidentifier not null,
    name varchar(250) not null,
    row_version rowversion not null,
    constraint pk_subdivisions primary key clustered(id)
)
go

-- default primary key value
alter table subdivisions
    add constraint df_subdivisions_id 
    default newsequentialid() for id
go

-- foreign key
alter table subdivisions
    add constraint fk_countries_to_subdivisions_id 
    foreign key (country_id) references countries(id)
go