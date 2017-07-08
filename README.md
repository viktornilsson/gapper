# Gapper

[![Build status](https://ci.appveyor.com/api/projects/status/et01c1o3klquficy?branch=release&svg=true)](https://ci.appveyor.com/project/viktornilsson91/gapper)

[![NuGet](https://img.shields.io/nuget/v/gapper.svg)](https://www.nuget.org/packages/gapper/)

![gapper-logo](gapper-logo.png)

# Install

The latest versions of the packages are available on NuGet. To install, run the following command.
```
PM> Install-Package Gapper
```

# Features

Helps you do simple T-SQL queries so you don't have to create SP:s for all simple calls.

# Usage

#### First reference the DapperService into your class.
```csharp
public class UserService : DapperService
```

#### Then we have a class looking like this.
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

#### And the last step is to use the wanted method, here we have a simple select.
```csharp
public User GetUser(int id)
{
    return Select<User>(new { id }).FirstOrDefault();
}
```

#### This results in follwing SQL query,
```
SELECT * FROM dbo.User Where Id = @id
```