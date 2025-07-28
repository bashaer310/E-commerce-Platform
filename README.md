# E-commerce Platform

This is a backend solution for an e-commerce platform, built with C#, ASP.NET, and PostgreSQL. The platform allows artists to showcase their artwork, while customers can purchase artworks and book art workshops. It includes core features like **user authentication**, **product management (artworks, workshops)**, **category management (types of artwork)**, and **order and booking processing**.

## Features

- **User Management**:
  - Register new User (Customer/Artist/Admin)
  - User authentication and authorization 
  - Update user profile
- **Product Management (Artworks, Workshops)**:
  - Create new product (title, description, price)
  - Retrieve product listing and product details with handling pagination and filtering
  - Update product information
  - Delete product
- **Category Management**:
  - Create new category (types of artwork)
  - Retrieve category listing and category details with handling pagination and filtering
  - Update category information
  - Delete category
- **Order Management**:
  - Create new order for purchased artworks
  - Retrieve order listing and order details with handling pagination and filtering
  - Update order status (pending, shipped, completed)
- **Booking Management**:
  - Create new booking for workshops
  - Retrieve booking listing and booking details with handling pagination and filtering
  - Update booking status (confirmed, canceled)
## Technologies Used
- .Net 8
- Entity Framework Core
- PostgreSQl
- JWT
- AutoMapper

## Getting Started

### 1. Clone the repository:

```bash
git clone https://github.com/bashaer310/E-commerce-Platform
```

### 2. Setup database

- Make sure PostgreSQL Server is running
- Create `appsettings.json` file
- Update the connection string in `appsettings.json`

```json
{
  "ConnectionStrings": {
    "Local": "Server=localhost;Database=ECommerceDb;User Id=your_username;Password=your_password;"
  }
}
```

- Run migrations to create database

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- Run the application

```bash
dotnet watch
```

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

The application is deployed and can be accessed at: [E-commerce-Platform](https://artify-backend.onrender.com/)

## Team Members

- **Lead**: [Abeer Aljohani](https://github.com/AbeerAljohanii)
- [Bashaer Alhuthali](https://github.com/bashaer310)
- [Danah Almalki](https://github.com/DanaAlmalki)
- [Manar Almalawi](https://github.com/mal-manar)
- [Shuaa Almarwani](https://github.com/Shuaa-99)

## License

This project is licensed under the MIT License.
