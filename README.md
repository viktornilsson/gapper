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

#### This results in following SQL query,
```
SELECT * FROM dbo.User Where Id = @id
```

## Rest of the operations.
```csharp
public int AddUser(User user)
{
    return Insert<User>(user);
}
```

```csharp
public int UpdateUser(User user)
{
    return Update<User>(user);
}
```

```csharp
public void DeleteUser(int id)
{
    Delete<User>(new { id });
}
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