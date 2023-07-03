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
- [Further Topics](#further-topics)
- [Author](#author)

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
- Reading List Management:
    - Manage Reading List creating, deleting, editing, viewing, adding to, and removing from them
- Book Reviews and Ratings:  
    - Rate and review books management such as creating, updating, and viewing
    - View reviews and ratings of other Patrons by book
- Notifications:
    - Send notifications to patrons about due dates, reserved books, or other important events whether by system or librarian.
- Book Recommendations:
    - Provide personalized book recommendations to patrons based on their borrowing history's top genres.

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

## Testing
Testing
The library management system has undergone thorough testing to ensure its functionality and reliability. The testing process includes various approaches to validate the system's behavior and ensure it meets the requirements. The following testing strategies and tools have been used:

Unit Testing
The library management system includes a suite of unit tests that verify the correctness of individual components and functions. These tests focus on testing specific functionalities in isolation, ensuring that each part of the system works as expected. The unit tests are focused on repositories and handlers. They are written using the built-in testing framework in Visual Studio(XUnit)

To run the unit tests, follow these steps:

1. Open the library management system solution in Visual Studio.
2. Build the solution to ensure all dependencies are resolved.
3. Open the Test Explorer window in Visual Studio (usually found under the "Test" menu).
4. Click the "Run All" button in the Test Explorer window to execute all unit tests.
5. Review the test results in the Test Explorer window to ensure all tests pass successfully.

## Further Topics

- Performance optimization
- Making RESTful Web APIs
- Platform Independence
- Service evolution
- Third-Party Integrations

## Author

This project was made by [Raneen Asia](https://github.com/RaneenTAsia)
