# E-commerce Platform

This is a backend solution for an e-commerce platform, built with C#, ASP.NET, and PostgreSQL. The platform allows artists to showcase their artwork, while customers can purchase artworks and book art workshops. It includes core features like **user authentication**, **product management (artworks, workshops)**, **category management (types of artwork)**, and **order and booking processing**.

## Features

- **User Management**:
  - Register new User (Customer/Artist)
  - User authentication and role-based authorization 
  - Update user profile
- **Product Management (Artworks, Workshops)**:
  - Artists can create, update, delete products (title, description, price)
  - Customers can retrieve products with pagination and filtering
  - View product details 
- **Category Management (types of artwork)**:
  - Admin can create, update, delete categories
  - Retrieve categories with pagination and filtering
- **Order Management**:
  - Customers can create new order for artworks
  - Customers can retrieve order history with pagination and filtering
  - Admin can update order status (pending, shipped, completed)
- **Booking Management**:
  -  Customers can create new booking for workshops
  -  Customers can retrieve booking history with pagination and filtering
  - Admin can update booking status (confirmed, canceled)
 
## Technologies Used
- Languages
  - C# – Programming language
- Frameworks
  - ASP.NET 8 – RESTful API framework
- Datebase
  - PostgreSQL – Relational database
- Packages
  - Entity Framework Core – ORM for database access
  - Npgsql – PostgreSQL provider for EF Core
  - JWT – Token-based authentication
  - AutoMapper – DTO and entity mapping
- Tools
  - Swagger / OpenAPI – API documentation
  - Postman – API testing

## Getting Started

1. Clone the repository:
  ```bash
  git clone https://github.com/bashaer310/E-commerce-Platform
  ```

2. Navigate to the project folder:
  ```bash
  E-commerce-Platform
  ```

3. Configure Database:
- Ensure PostgreSQL is running.
- Create `appsettings.json` file in the root folder.
- Add your connection string:

```json
{
  "ConnectionStrings": {
    "Local": "Server=localhost;Database=ECommerceDb;User Id=your_username;Password=your_password;"
  }
}
```

4. Apply migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- Run the application:

```bash
dotnet watch
```
5. Test the API
   
   Use tools like Postman to test the endpoints.

## Project structure

```bash
E-commerce-platform/
├── Src/
│   ├── Controllers/        # API controllers for handling requests and responses
│   ├── Database/           # DbContext and database configurations
│   ├── DTOs/               # Data Transfer Objects
│   ├── Entities/           # Database entities
│   ├── Middleware/         # Request/response logging and error handling
│   ├── Repositories/       # Repository layer for database operations
│   ├── Services/           # Business logic layer
│   ├── Utils/              # Utility functions and common logic
│   ├── Migrations/         # Entity Framework Core migrations
│   └── Program.cs          # Application entry point
├── appsettings.json        # Configuration file for environment variables and DB connection
```

## API Endpoints
All the API Endpoints are documented and can be accessed at:

## Deployment
The application is deployed and can be accessed at: [E-commerce-Platform](https://artify-backend-project.onrender.com/)

## Team Members

- **Lead**: [Abeer Aljohani](https://github.com/AbeerAljohanii)
- [Bashaer Alhuthali](https://github.com/bashaer310)
- [Danah Almalki](https://github.com/DanaAlmalki)
- [Manar Almalawi](https://github.com/mal-manar)
- [Shuaa Almarwani](https://github.com/Shuaa-99)

## License

This project is licensed under the MIT License.
