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
Extract the pegawai_app.dacpac file to get the database
```

To access Swagger UI documentation please follow this link https://localhost:{YOUR_PORT}/swagger/index.html
