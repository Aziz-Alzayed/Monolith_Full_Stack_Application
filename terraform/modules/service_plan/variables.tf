# variables.tf

variable "name" {
  description = "The name of the App Service Plan"
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group where the App Service Plan will be created"
  type        = string
}

variable "location" {
  description = "The location/region where the App Service Plan will be created"
  type        = string
}

variable "os_type" {
  description = "The kind of the App Service Plan (e.g., 'Linux' or 'Windows')"
  type        = string
  default     = "Linux"
}

variable "sku_name" {
  description = "The SKU tier for the App Service Plan (e.g., 'S1', 'P1')"
  type        = string
}

variable "tags" {
  description = "A map of tags to assign to the App Service Plan"
  type        = map(string)
  default     = {}
}
