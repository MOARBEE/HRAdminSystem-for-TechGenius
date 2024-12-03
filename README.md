# HRAdminSystem for TechGenius
This HR Administration System is designed to manage employee details and their associated departments efficiently. Developed as part of a tech test for Tech Genius, it demonstrates capabilities in handling secure login systems and CRUD operations within an ASP.NET environment.

#Features
User Login: Secure authentication for different roles.
Employees List View: Display all employees with options to edit or add new entries.
Employee Create/Edit View: Allows for the creation and modification of employee details.
Departments List View: View all departments with edit and create capabilities.
Departments Create/Edit View: Manage department details.


#Built With:
IDE: Visual Studio 2022
Language: C#
Framework: ASP.NET Core (Razor Pages)
Styling: Bootstrap
Database: SQLite
ORM: Entity Framework Core
Validation: Frontend and Backend data validation
Authentication: Secure authentication with hashed password storage


#Prerequisites
Before you begin, ensure you have met the following requirements:
.NET SDK (compatible with ASP.NET Core)
Entity Framework Core
SQLite


#Installation
Clone the repository to your local machine:
git clone https://github.com/MOARBEE/HRAdminSystem-for-TechGenius.git

Navigate to the project directory:
cd HRAdminSystem


#Configuration
Update the appsettings.json with your SQLite connection string.
Apply migrations to set up your database schema:
dotnet ef migrations add InitialCreate
dotnet ef database update


#Running the Project
To run the project locally, use the following command in the root directory:
dotnet run
This will start the application on http://localhost:5000 (or a different port configured in your settings).


#Usage
Log in using the provided super user credentials:

Username: hradmin@test.com
Password: TestPass1234!
Navigate through the application to manage employees and departments as per your role privileges.

Username: employee email *
Password: Password123#
Navigate through the application as that employee and only able to access that employees specific details.

#Best Practices
This project adheres to best coding practices, focusing on readability, maintainability, and secure coding standards. Ensure you follow these principles when modifying or extending the project.
