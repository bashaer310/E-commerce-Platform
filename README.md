# E-commerce Platform

This is a backend solution for an e-commerce platform, built with C#, ASP.NET, and PostgreSQL. The platform allows artists to showcase their artwork, while customers can purchase artworks and book art workshops. It includes core features like **user authentication**, **product management (artworks, workshops)**, **category management (types of artwork)**, and **order and booking processing**.

## Features

- **User Management**:
  - **User Registration** - users can sign up (Customer/Artist)
  - **Authentication & Authorization** - users can login with role-based access
  - **Profile Management** - users can update their prodile
- **Product Management (Artworks, Workshops)**:
  - **Product Management** - artists can create, update, delete products (title, description, price)
  - **Product Browsing** - customers can retrieve products with pagination and filtering
  - **Product Details View** - users can view product details 
- **Category Management (types of artwork)**:
  - **Category Management** - admin can create, update, delete categories
  - **Category Retrieval** - retrieve categories with pagination and filtering
- **Order Management**:
  - **Order Creation** - customers can create new order for artworks
  - **Order History** - customers can retrieve order history with pagination and filtering
  - **Order Status Management** - admin can update order status (pending, shipped, completed)
- **Booking Management**:
  - **Booking Creation** - customers can create new booking for workshops
  - **Booking History** - customers can retrieve booking history with pagination and filtering
  - **Booking Status Management** - admin can update booking status (confirmed, canceled)
 
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
