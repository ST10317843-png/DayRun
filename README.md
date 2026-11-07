# DayRun

# DayRun API

A RESTful API for managing road running, walking, and cycling events in South Africa. Built with **ASP.NET Core 8**, **Entity Framework Core**, **SQL Server (LocalDB)**, and **Cookie Authentication**.

## Roles

| Role | Capabilities |
|------|--------------|
| **Organiser** | Create, update, delete events; define categories; view all enrolments for own events; capture results |
| **Participant** | Register, log in, browse events, enrol in categories, view own enrolments, view own results |

## Setup Instructions

### Prerequisites
- Visual Studio 2022 with **ASP.NET and web development** workload
- .NET 8.0 SDK
- SQL Server Express LocalDB
- SQL Server Management Studio (SSMS)
git clone <your-repo-url>
cd DayRun

2. Create the LocalDB instance (once):
SqlLocalDB.exe create "api_booth" -s

3. Open `DayRun.sln` in Visual Studio.

4. Set **DayRun.API** as Startup Project (right-click → Set as Startup Project).

5. Open **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console).

6. Set **Default project** to `DayRun.API`.

7. Apply migrations:
8. Update-Database
9. 
### Steps

1. Clone the repository:
