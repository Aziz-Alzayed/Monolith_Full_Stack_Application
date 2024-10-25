
# CI/CD Pipelines Documentation

This repository contains CI/CD pipelines for provisioning infrastructure and deploying a Full-Stack To-Do Manager application. The pipelines cover:

1. **Terraform Infrastructure**: Infrastructure provisioning on Azure.
2. **Backend API**: CI/CD for the backend API using .NET.
3. **Frontend UI**: CI/CD for the frontend application using Node.js.
4. **Function App (Timer Trigger)**: CI/CD for Azure Function App (Timer Trigger).

## Table of Contents

- [Folder Structure](#folder-structure)
- [Pipeline Overviews](#pipeline-overviews)
  - [1. Terraform Infrastructure Pipeline](#1-terraform-infrastructure-pipeline)
  - [2. Backend API Pipeline](#2-backend-api-pipeline)
  - [3. Frontend UI Pipeline](#3-frontend-ui-pipeline)
  - [4. Function App Pipeline](#4-function-app-pipeline)
- [How to Trigger Pipelines](#how-to-trigger-pipelines)
- [Secrets and Environment Variables](#secrets-and-environment-variables)

## Folder Structure

```
.
│   README.md                      # Documentation for CI/CD Pipelines
│   .github/workflows/             # GitHub workflows for CI/CD
│
├── terraform/                     # Terraform Infrastructure code
├── api/                           # Backend API code (e.g., ASP.NET Core)
│   └── FSTD/                      # Specific backend project directory
├── ui/                            # Frontend UI code (React or Angular)
├── services/                      # Function app and microservices
│   └── FSTD.TimerTriggers/        # Azure Function App Timer Trigger code
```

---

## Pipeline Overviews

### 1. Terraform Infrastructure Pipeline

- **Path**: `.github/workflows/terraform.yml`
- **Purpose**: Provision and manage Azure infrastructure using Terraform.
- **Triggers**:
  - **Manual**: `workflow_dispatch`
  - **Push to `terraform/**` path**: Automates infrastructure changes when Terraform files are modified.

**Steps**:
1. **Initialize Terraform**: Set up the working directory and download providers.
2. **Plan**: Generate and display the execution plan.
3. **Apply**: Apply the changes to the Azure infrastructure upon confirmation.

### 2. Backend API Pipeline

- **Path**: `.github/workflows/API-Workflow.yml`
- **Purpose**: Build, test, and deploy the backend API to Azure.
- **Triggers**:
  - **On Push/Merge to `main`**: For changes in `api/FSTD/**`.
  - **Manual**: `workflow_dispatch`

**Steps**:
1. **Set Environment Variables**: Define the repository name, Azure web app name, and file path.
2. **Build & Test**: Use the reusable workflow to build the .NET solution and run tests.
3. **Deploy to Azure**: Use a reusable workflow to push the Docker image to Azure Container Registry (ACR) and deploy to the Azure Web App.

### 3. Frontend UI Pipeline

- **Path**: `.github/workflows/UI-Workflow.yml`
- **Purpose**: Build, test, and deploy the frontend UI to Azure.
- **Triggers**:
  - **Manual**: `workflow_dispatch`
  - **On Push to `main`**: For final deployment.

**Steps**:
1. **Set Environment Variables**: Define the repository name and API URL for build arguments.
2. **Build & Test**: Use Node.js to build and test the frontend codebase.
3. **Deploy to Azure**: Push the Docker image to ACR and deploy to Azure Web App.

### 4. Function App Pipeline

- **Path**: `.github/workflows/TriggerFunction-Workflow.yml`
- **Purpose**: Build, test, and deploy the Azure Function App Timer Trigger.
- **Triggers**:
  - **On Push/Merge to `main`**: For changes in `services/FSTD.TimerTriggers/**`.
  - **On Tagging with `triggerFunction-v*`**: For versioned releases.

**Steps**:
1. **Set Environment Variables**: Define the repository name and Azure web app name.
2. **Build & Test**: Use the reusable workflow to build and test the function app.
3. **Deploy to Azure**: Push the Docker image to ACR and deploy to the Azure Web App.

---

## How to Trigger Pipelines

- **Manual Trigger**: Most pipelines can be manually triggered using the GitHub Actions tab in the repository.
- **Push Trigger**: Pipelines will execute on any push to `main` or to specific directories.

## Secrets and Environment Variables

The workflows utilize the following GitHub Secrets:

- **`ACR_LOGIN_SERVER`**: Azure Container Registry login server.
- **`ACR_USERNAME`**: Username for ACR access.
- **`ACR_PASSWORD`**: Password for ACR access.
- **`AZURE_WEBAPP_NAME`**: Name of the Azure Web App for deployment.
- **`API_BACKEND_URL`**: Backend API URL for frontend build arguments.

Ensure these secrets are properly set in your GitHub repository settings.

---
## Author & Contact
**Author:** Aziz Alzayed
**Email:** aziz.alzayed@a-fitech.com