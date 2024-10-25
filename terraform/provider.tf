terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 2.43.0"
    }
  }
}

provider "azurerm" {
  features {}

  # Azure Subscription ID
  subscription_id = var.subscription_id

  # Azure Client ID (Application ID for Service Principal)
  client_id       = var.client_id

  # Azure Client Secret (Password for Service Principal)
  client_secret   = var.client_secret

  # Azure Tenant ID
  tenant_id       = var.tenant_id
}
