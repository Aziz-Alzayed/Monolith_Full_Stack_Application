
variable "subscription_id" {
  type        = string
  description = "Azure Subscription ID"
  sensitive   = true
}

variable "client_id" {
  type        = string
  description = "Azure Client ID"
  sensitive   = true
}

variable "client_secret" {
  type        = string
  description = "Azure Client Secret"
  sensitive   = true
}

variable "tenant_id" {
  type        = string
  description = "Azure Tenant ID"
  sensitive   = true
}

variable "sql_server_login" {
  description = "Login for the Azure SQL Server"
  type        = string
  sensitive   = true
}

variable "sql_server_login_password" {
  description = "Password for the Azure SQL Server"
  type        = string
  sensitive   = true
}
