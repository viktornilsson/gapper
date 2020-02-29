# Gapper

[![Build status](https://ci.appveyor.com/api/projects/status/o52ul25c5yp5i4so/branch/release?svg=true)](https://ci.appveyor.com/project/viktornilsson91/gapper/branch/release)

[![NuGet](https://img.shields.io/nuget/v/gapper.svg)](https://www.nuget.org/packages/gapper/)

![gapper-logo](gapper-logo.png)

# Install

The latest versions of the packages are available on NuGet. To install, run the following command.
```
PM> Install-Package Gapper
```

## Features

Helps you do simple T-SQL queries so you don't have to create SP:s for all simple calls.

- Can run sync or async.
- No need to create SPs for simple calls.

## IDBConnection extension usage

First reference the namespace Gapper into your class.

```csharp
using Gapper;
```

First create a class that is a direct copy of the SQL table. This is important.

```csharp
public class User
{
    public User()
    {
        Id = -1;
        CreatedDate = DateTime.UtcNow;
    }
    
    public int Id { get; set; }    
    public string Name { get; set; }    
    public int Age { get; set; }    
}
```

### Insert
Inserts automatically filter away the `Id` property, so if your SQL table have different Key it can be a problem.

```csharp
var user = new User 
{ 
    Name = "Sten", 
    Age = 20 
};

var id = connection
    .Insert(user)
    .Execute();
```

### Select

```csharp
var user = connection
    .Select<User>()
    .Where(x => x.Name).EqualTo("Viktor")
    .FirstOrDefault();
```

### Update

```csharp
connection
    .Update<User>()
    .Set(x => x.Name, "Viktor")
    .Where(x => x.Id).EqualTo(1)
    .Execute();
```

### Delete

```csharp
connection
    .Delete<User>()
    .Where(x => x.Id).EqualTo(newId)
    .Execute();
```

## Where clause conditions

These conditions are available at the moment.

|           Conditions  |
|---------------------- |
| EqualTo               |
| NotEqualTo            |
| GreaterThan           |
| LessThan              |
| GreaterThanOrEqualTo  |
| LessThanOrEqualTo     |
| Like                  |


## Table name generation

The table name is generated automatically from the class schema and name like this.

```csharp
namespace Gapper.Models
{
    public class User
...    
```

``` sql
[dbo].[User]
```

## Table attribute 
Can be used to specify the unique table name.
```csharp
[Table(Name = "tblUser", Schema = "users")]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```    

```    