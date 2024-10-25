
# Example of Terraform Infrastructure - Full-Stack To-Do Manager

This repository contains an example of my skills in Terraform configurations to provision and manage the infrastructure for the Full-Stack To-Do Manager application on **Azure**. The infrastructure includes various services such as app services, function apps, SQL servers, container registries, and more, along with role-based access control (RBAC) and policies for compliance.

## Table of Contents

- [Folder Structure](#folder-structure)
- [Prerequisites](#prerequisites)
- [How to Use](#how-to-use)
- [Modules](#modules)
- [Variables](#variables)
- [Policies](#policies)
- [Contributing](#contributing)

## Folder Structure

This is the structure of the project:

```
.
│   .gitignore                  # Git configuration to ignore unnecessary files
│   .terraform.lock.hcl          # Terraform lock file for dependencies
│   main.tf                      # The root Terraform configuration file
│   provider.tf                  # Configuration for the Azure provider
│   secrets.tfvars               # Variables file for sensitive information (not included in version control)
│   terraform.tfstate            # Terraform state file (not included in version control)
│   terraform.tfstate.backup     # Backup of the Terraform state file
│   variables.tf                 # Global variables used throughout the project
│
│
├── modules                      # Reusable Terraform modules
│   ├── container_registry       # Module for creating container registries
│   ├── function_app             # Module for Azure Function Apps
│   ├── linux_app_service        # Module for Linux-based App Services
│   ├── resource_group           # Module for creating resource groups
│   ├── service_plan             # Module for creating App Service Plans
│   ├── sql_db                   # Module for creating SQL Databases
│   ├── sql_server               # Module for creating SQL Servers
│   └── storage_account          # Module for creating Azure Storage Accounts
│
└── policies                     # Azure Policies for enforcing compliance
    └── require_tags.tf          # Policy to ensure resources have tags
```

### Key Directories and Files:
- **`main.tf`**: The entry point for Terraform that calls all resources and modules.
- **`provider.tf`**: Configures the Azure provider with authentication details.
- **`secrets.tfvars`**: Stores sensitive information (such as client secrets) that should not be hardcoded in the codebase.
- **`modules/`**: Contains reusable modules to provision specific Azure services.
- **`policies/`**: Contains Azure policies, such as requiring resources to have tags.

## Prerequisites

Ensure you have the following prerequisites set up:

1. **Azure Subscription**: You will need an Azure subscription where the infrastructure will be provisioned.
2. **Terraform**: Ensure you have [Terraform](https://www.terraform.io/downloads.html) installed (v0.12 or later).
3. **Azure CLI**: Install the [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) to authenticate and interact with your Azure account.
4. **Service Principal**: Create an Azure Service Principal and note the `client_id`, `client_secret`, `subscription_id`, and `tenant_id`.

## How to Use

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Configure Provider Authentication**:
   You need to configure the Azure provider to authenticate using a Service Principal. Ensure you add the following details in your `secrets.tfvars` file:

   ```hcl
   subscription_id = "your-subscription-id"
   client_id       = "your-client-id"
   client_secret   = "your-client-secret"
   tenant_id       = "your-tenant-id"
   ```

3. **Initialize Terraform**:
   Initialize the Terraform working directory and download the necessary provider plugins.
   ```bash
   terraform init
   ```

4. **Plan and Apply**:
   - **Plan**: Review the changes Terraform will apply:
     ```bash
     terraform plan -var-file="secrets.tfvars"
     ```
   - **Apply**: Apply the infrastructure changes:
     ```bash
     terraform apply -var-file="secrets.tfvars"
     ```

## Modules

The repository uses several reusable Terraform modules for managing Azure resources. Here’s a brief overview of each module:

- **`container_registry`**: Provisions an Azure Container Registry (ACR) for storing container images.
- **`function_app`**: Sets up an Azure Function App for running serverless functions.
- **`linux_app_service`**: Configures an Azure App Service running on Linux.
- **`resource_group`**: Provisions Azure Resource Groups.
- **`service_plan`**: Creates an App Service Plan for hosting applications.
- **`sql_db`**: Creates SQL databases.
- **`sql_server`**: Provisions SQL Servers.
- **`storage_account`**: Creates Azure Storage Accounts for blob, file, table, and queue storage.

## Variables

Key variables used throughout the project include:

- **`subscription_id`**: The Azure Subscription ID where resources will be provisioned.
- **`client_id`**: The Service Principal Application ID.
- **`client_secret`**: The Service Principal Secret Key.
- **`tenant_id`**: The Azure Active Directory Tenant ID.
- **`tags`**: A map of key-value pairs used for tagging resources.

## Security

Sensitive information, such as Service Principal credentials and SQL server passwords, are stored in `secrets.tfvars` and marked as **sensitive** in the variables files. Ensure that this file is never committed to version control and is stored securely.

## Contributing

If you'd like to contribute, please submit a pull request. Ensure you follow best practices when writing Terraform code, and ensure that sensitive information is never hardcoded in the codebase.

---

