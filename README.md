# Gapper

[![Build status](https://ci.appveyor.com/api/projects/status/o52ul25c5yp5i4so/branch/release?svg=true)](https://ci.appveyor.com/project/viktornilsson91/gapper/branch/release)

[![NuGet](https://img.shields.io/nuget/v/gapper.svg)](https://www.nuget.org/packages/gapper/)

![gapper-logo](gapper-logo.png)

# Install

The latest versions of the packages are available on NuGet. To install, run the following command.
```
PM> Install-Package Gapper
```

# Features

Helps you do simple T-SQL queries so you don't have to create SP:s for all simple calls.

# IDBConnection extension usage

#### First reference the namespace Gapper into your class.
```csharp
using Gapper;
```

#### Then you can use it like this.

```csharp
var user = new User() { Id = 1, Name = "Sten", Age = 20 };

var id = connection
    .Insert(user)
    .Execute();
```

```csharp
var user = connection
    .Select<User>()
    .Where(nameof(User.Name)).EqualTo("Viktor")
    .FirstOrDefault();
```

```csharp
connection
    .Update<User>(new UpdateValues
    {
        { nameof(User.Name), "Viktor" }
    })
    .Where(nameof(User.Id)).EqualTo(1)
    .Execute();
```

```csharp
connection
    .Delete<User>()
    .Where(nameof(User.Id)).EqualTo(newId)
    .Execute();
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