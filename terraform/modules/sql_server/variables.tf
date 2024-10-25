variable "name" {
  description = "The name of the SQL server"
}

variable "resource_group_name" {
  description = "The name of the resource group in which to create the SQL server"
}

variable "location" {
  description = "The Azure region where the SQL server should be created"
}

variable "admin_login" {
  description = "The administrator login name for the SQL server"
}

variable "admin_password" {
  description = "The administrator login password for the SQL server"
}

variable "sql_version" {
  description = "The version of the SQL server"
  default     = "12.0" # Default to SQL Server 2016
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource"
  default     = {}
}
