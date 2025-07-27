# E-commerce Platform

This is a backend solution for an e-commerce platform, built with C#, ASP.NET 8, and PostgreSQL. The platform allows artists to showcase their artwork, while customers can purchase artworks and book art workshops. It includes core features like **user authentication**, **product management (artworks, workshops)**, **category management (types of artwork)**, and **order and booking processing**.

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
- **.Net 8**: Web API Framework
- **Entity Framework Core**: ORM for database interactions
- **PostgreSQl**: Relational database for storing data
- **JWT**: For user authentication and authorization
- **AutoMapper**: For object mapping
- **Swagger**: API documentation


## Getting Started

### 1. Clone the repository:

```bash
git clone https://github.com/AbeerAljohanii/sda-3-online-Backend_Teamwork
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

The API will be available at: `http://localhost:5125`

### Swagger

- Navigate to `http://localhost:5125/swagger/index.html` to explore the API endpoints.

## Project structure

```bash
|-- Controllers: API controllers with request and response
|-- Database # DbContext and Database Configurations
|-- DTOs # Data Transfer Objects
|-- Entities # Database Entities (User, ArtWorks, Category, Order)
|-- Middleware # Logging request, response and Error Handler
|-- Repositories # Repository Layer for database operations
|-- Services # Business Logic Layer
|-- Utils # Common logics
|-- Migrations # Entity Framework Migrations
|-- Program.cs # Application Entry Point
```

## API Endpoints
All the API Endpoints are documented can be accessed at postman:

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
