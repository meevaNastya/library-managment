# Library Management

Library Management is a simple object-oriented .NET 8 project for managing books in a library.

The project does not contain REST API, Swagger, JWT, database access, or UI code. It focuses on domain models, repositories, services, business rules, and unit tests.

## Features

- Register users with one or more roles.
- Store the current user in a user session.
- Create requests to take a book.
- Create requests to return a book.
- Create requests to add a writer's book to the library.
- Approve or reject requests as a librarian.
- Automatically approve requests when the current user is also a librarian.

## Roles

- `Reader` creates requests to take and return books.
- `Writer` creates requests to add own books.
- `Librarian` approves or rejects created requests.

A user can have several roles at the same time.

## Business Rules

- User names are unique.
- A writer can create add-book requests only for own books.
- The number of book copies cannot exceed the copies limit.
- A reader cannot create a take-book request for a book that is already taken by this reader.
- A reader can take the same book again after returning it.
- Requests created by users with the `Librarian` role are approved automatically.
- Book copies are checked when a request is approved.

## Project Structure

```text
LibraryManagement.Core
  Exceptions
  Models
  Repositories
  Services

LibraryManagement.Tests
  Models
  Repositories
  Services
```

## Main Classes

- `User` describes a library user.
- `Book` describes a book and its copy count.
- `BookRequest` describes a request and its status.
- `UserRepository`, `BookRepository`, and `BookRequestRepository` store objects in memory.
- `UserSession` stores the current user.
- `UserService` registers users and manages login/logout.
- `LibraryService` contains the main library business logic.

## Requirements

- .NET 8 SDK

## Run Tests

```powershell
dotnet test .\LibraryManagement.sln
```

## Check Coverage

```powershell
dotnet test .\LibraryManagement.sln --collect:"XPlat Code Coverage"
```

Current checked result:

```text
Line coverage: 89.81%
Branch coverage: 70.00%
```

## Check Formatting

```powershell
dotnet format .\LibraryManagement.sln --verify-no-changes --verbosity minimal
```
