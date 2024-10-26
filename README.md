
# Monolith Full-Stack Application

## Overview

This is a comprehensive **Monolithic Full-Stack Application** built using modern web technologies, combining a powerful **.NET 8 API** backend and a **React.js** frontend, fully integrated with **MobX** and **Redux ToolKit** for state management. The application follows **Clean Architecture** principles with **CQRS** and **SOLID** principles, ensuring scalability, maintainability, and testability. The infrastructure is managed using **Terraform** for full automation, and the entire solution is containerized with **Docker** for easy deployment.

## Key Features

### Backend - .NET 8 API:
- **CQRS & Mediator Pattern**: Follows the Command-Query Responsibility Segregation (CQRS) pattern with the **Mediator** for managing requests.
- **FluentValidation**: Ensures strong input validation for both commands and queries, helping maintain robust API integrity.
- **AutoMapper**: For automatic mapping between domain models and Data Transfer Objects (DTOs).
- **Entity Framework (EF)**: For database management and interactions, following **SOLID** principles.
- **Authentication**: JWT-based authentication ensuring secure access to API resources.
- **Email Services**: Integrated email functionality to handle verifications and notifications.
- **Unit Tests & Integration Tests**: Implemented to ensure stability and code integrity, following best testing practices.
- **Containerization**: Fully containerized using Docker for easy scalability and deployment.

### Frontend - React.js:
- **MobX and Redux Tool Kit State Management**: A scalable and efficient solution for handling global state.
- **Ant Design & TailwindCSS**: Modern UI/UX with **Ant Design** components and **TailwindCSS** for styling.
- **i18n Localization**: Support for multiple languages, allowing for easy expansion to other markets.
- **JWT Authentication**: Secure login and session handling using JWT tokens.
- **Dynamic Routing with React Router DOM**: Provides seamless navigation between different parts of the application.
- **Task Management Module**: Add, edit, and manage tasks in a user-friendly interface.
- **Role-Based Access Control (RBAC)**: Super Admin and User roles with different access permissions.

### Infrastructure:
- **Terraform**: Infrastructure as Code (IaC) allowing automated provisioning and management of cloud infrastructure.
- **CI/CD Pipeline**: Integrated DevOps pipeline for continuous integration and deployment.
- **Docker**: Used to containerize the entire application, ensuring easy deployment across environments.

## Dockerization

The entire application, both frontend and backend, is fully containerized using Docker. This ensures the application can be deployed across various environments without compatibility issues.

### Environment Variables:
Create a `.env.local` file in the frontend with the following content to set the backend API URL:
```env
VITE_API_URL=https://localhost:7299/api
```

## Getting Started

### Prerequisites:
- **.NET 8 SDK** for backend development.
- **Node.js** (v18 or later) for the frontend.
- **Docker** installed for containerization.
- **Terraform** for infrastructure management.

### Running Locally:
1. Clone the repository.
2. Navigate to the backend project directory and run:
   ```bash
   dotnet build
   dotnet run
   ```
3. Navigate to the frontend project directory, install dependencies, and start the dev server:
   ```bash
   npm install
   npm run start:dev
   ```

4. To run the application in Docker, build and start the containers:
   ```bash
   docker-compose up --build
   ```

## Tests

This project includes **Unit Tests**, **Functional Tests**, and **Integration Tests**:
- **Unit Tests**: Validate individual pieces of code logic.
- **Functional Tests**: Ensure the core functionality of the application.
- **Integration Tests**: Test interactions between different components of the system.

To run the tests for the backend:
```bash
dotnet test
```

## Infrastructure Automation

The entire infrastructure is provisioned and managed using **Terraform**. With Terraform, you can define and provision your infrastructure in a simple, declarative way.

```bash
# Initialize Terraform
terraform init

# Apply the infrastructure
terraform apply
```

## Conclusion

This project follows modern architecture principles and leverages advanced technologies to ensure scalability, maintainability, and ease of deployment. With its robust **CQRS** structure, **SOLID** principles, and containerized infrastructure, it’s an excellent foundation for building scalable monolithic applications.

## Contact
For questions, suggestions, or feedback, feel free to reach out:
- **Email:** aziz.alzayed@A-FiTech.com
- **Website:** www.A-FiTech.com/about
- **LinkedIn:** [LinkedIn Profile](https://www.linkedin.com/in/abdul-aziz-alzayed/)

## License and Rights
This project is licensed under the terms of the license specified in the LICENSE file. **All rights reserved by Aziz Alzayed**. Unauthorized reproduction, use, or modification of this code without prior permission is strictly prohibited.

---
Thank you for using and supporting **Monolith Full-Stack Application**!
