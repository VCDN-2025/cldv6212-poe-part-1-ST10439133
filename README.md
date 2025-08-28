-ABCRetail Eccommerce Website-

Overview:
ABCRetailPOE is an ASP.NET Core MVC application designed for ABC Retail. The project demonstrates the use of Azure Storage solutions, including Table Storage for customer and product information and Blob/File storage for managing product images and related assets. It supports CRUD operations and provides a scalable foundation for modern retail systems.

Features:
-Manage customers, products, orders, and contracts
-Integration with Azure Table Storage
-File and image handling with Azure Blob/File Storage
-MVC architecture with Models, Views, and Controllers
-Configuration through appsettings.json and environment-specific settings
-Bootstrap-based responsive UI

Project Structure:
-Controllers/: Handles incoming requests and coordinates responses
-Models/: Defines data structures such as Customer, Product, Order, and Contract
-Services/: Business logic and Azure integration logic
-Views/: Razor views for each module (Customer, Product, Order, Contract, Home)
-wwwroot/: Static assets including CSS, JavaScript, images, and libraries
-appsettings.json: Central configuration file for application settings

Requirements:
-.NET 8 SDK
-Azure Storage account (with Blob, File, and Table Storage enabled)
-Visual Studio 2022 or Visual Studio Code
-Internet connection for dependency resolution

Setup Instructions:
1.Clone the repository or extract the project archive.
2.Open the solution file in Visual Studio or run from the command line.
3.Configure the connection strings in appsettings.json to point to your Azure Storage account.
4.Restore dependencies: dotnet restore
5.Build the project: dotnet build
6.Run the project: dotnet run
7.Access the application at https://localhost:5001 or the URL provided in your terminal.

License:
This project is developed for academic purposes as part of a practical assignment for ABC Retail. Redistribution or commercial use should be clarified with the project owner.
Visual Studio 2022 or Visual Studio Code

Developed by: ST10439133
