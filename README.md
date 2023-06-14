# Libro
# Library Management System

## Table of Contents

- [Project Description](#project-description)
- [Project Setup](#project-setup)
- [Usage](#usage)
- [Features](#features)
- [Architecture and Design](#architecture-and-design)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [Testing](#testing)
- [License](#license)
- [Contact Information](#contact-information)
- [Acknowledgments](#acknowledgments)
- [Troubleshooting](#troubleshooting)

## Project Description

Salam, This repository contains a project called Libro that uses Asp.Net Core to query and manipulate data from a database by utilizing Web Apis that provide endpoints. The project is used to challenge interns of their complete knowledge of Asp.Net Core Web Apis that they have carefully learned throught internship, such as controllers, AutoMappers, Fluent Validations, MediatR, Serilog, AutoDependencies, Clean Code Architecture, Solid principles and more.

## Project Setup

1. Clone the repository: `git clone https://github.com/RaneenTAsia/Libro.git`
2. Install dependencies: `install`
3. Configure the database connection in `appsettings.Development`.
4. Run the application.

## Usage

- Allow Administrators, Librarians, and Patrons to have enjoyable access to a managed system of books and authors while maintaining appropriate access.

## Features

- User Registration and Authentication:
    - Register
    - Login
    - Specify access level
- Book searching and browsing:
    - Search for books by title, author, or genre
    - Browse books
    - View book details.
- Book Transactions:
    - Reserve book
    - Checkout book
    - Return book
    - Track overdue books
- Patron Profiles:
    - View Profile
    - Edit Profile
- Book And Author Management:
    - Manage Book adding, updating, and deleting books
    - Manage Author adding, updating, and deleting authors
    - Manage Librarian Accounts for Administrators

## Architecture and Design

- The library management system follows a Clean Architecture approach which utilizes MediatR to ensure Solid principles.
- For a further visualization and clarity, here is a [Software Design Document](https://docs.google.com/document/d/1pzMpLAQu7OtKgNb_Spwy1E_3IE00YjU7NuJZ89VISx8/edit)

## API Documentation

The library management system provides a RESTful API for developers to integrate into their applications. Visit [API Documentation](api-docs.md) for detailed information on available endpoints, request/response formats, and authentication mechanisms.

## Contributing

Contributions are welcome! To contribute to the project, please follow these guidelines:
1. Fork the repository and create a new branch for your feature or bug fix.
2. Ensure your code adheres to the project's code style and conventions.
3. Submit a pull request, clearly explaining the changes you have made.

## Further Topics

- Performance optimization
- Making RESTful Web APIs
- Platform Independence
- Service evolution
- Third-Party Integrations

## Author

This project was made by [Raneen Asia](https://github.com/RaneenTAsia)
