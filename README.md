# HotelBookingAPI

**HotelBookingAPI** is a fully functional RESTful Web API built with **ASP.NET Core** and **Entity Framework Core** for managing hotel bookings. The project implements **user authentication, role-based authorization, room and booking management**, and secure handling of sensitive information such as passwords using **BCrypt**. It also leverages **JWT (JSON Web Tokens)** for authentication and supports **Swagger UI** for API exploration.

This project is designed as a complete backend solution for a hotel management system.

---

## Features

### User & Role Management
- **Register/Login users** with secure password hashing (BCrypt).  
- **Role-based access control** to restrict endpoints based on user roles (Admin, Staff, Guest).  
- CRUD operations for Users and Roles.  

### Rooms & Booking
- Manage **Room Types** (e.g., Single, Double, Suite).  
- CRUD operations for **Rooms** with availability tracking.  
- Create, update, and cancel **Bookings**.  
- Generate booking confirmations for users.  

### Authentication & Security
- JWT-based authentication (`Bearer Token`) for secure API access.  
- Role-based authorization at **method level** using `[Authorize(Roles = "...")]`.  
- Passwords are securely hashed using **BCrypt** before storage.  

### Technologies Used
- **ASP.NET Core ** – API framework.  
- **Entity Framework Core** – ORM for database operations.  
- **SQL Server** – Relational database for storing users, roles, rooms, and bookings.  
- **Swagger** – Auto-generated interactive API documentation.  
- **Moq & NUnit** – Unit testing framework and mocking for service layer tests.  

### API Endpoints
- `/api/User` – User management (CRUD).  
- `/api/Auth/login` – Authenticate user and generate JWT token.  
- `/api/RoomType` – Manage room types.  
- `/api/Room` – Manage rooms.  
- `/api/Booking` – Manage bookings.  

### Unit Testing
- Includes unit tests for **UserService** using NUnit & Moq.  
- Tests cover login authentication.



