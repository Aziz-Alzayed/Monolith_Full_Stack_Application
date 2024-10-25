variable "acr_name" {
  description = "The name of the container registry. Must be unique."
}

variable "resource_group_name" {
  description = "The name of the resource group."
}

variable "location" {
  description = "The Azure region where the container registry should be created."
}

variable "sku" {
  description = "The SKU name of the container registry."
  default     = "Basic"
}

variable "admin_enabled" {
  description = "Indicates whether the admin user is enabled."
  default     = true
}

variable "tags" {
  description = "A mapping of tags to assign to the resource."
  type        = map(string)
  default     = {}
}
