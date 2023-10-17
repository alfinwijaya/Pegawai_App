# Simple Pegawai App

> Simple Web API to demonstrate relationship between tables

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)

## General info
Master table : Employee, IdCard, Division, and Task

Relation:
* 1 - 1 => Employee - IdCard
* 1- n => Division - Employee
* n - n => Employee - Task

## Technologies
Project is created with:
* C# 
* SQL Server

## Setup
To run this project, make sure you have SQL Server installed on your machine:
```
Run the scripts that has been provided in create_db.sql and create_table.sql
```

To access Swagger UI documentation please follow this link https://localhost:{port}/swagger/index.html
