variable "name" {
  description = "The name of the Function App"
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group"
  type        = string
}

variable "location" {
  description = "The location of the Function App"
  type        = string
}

variable "service_plan_id" {
  description = "The ID of the App Service Plan"
  type        = string
}

variable "storage_account_name" {
  description = "The name of the Storage Account"
  type        = string
}

variable "storage_account_access_key" {
  description = "The access key of the Storage Account"
  type        = string
}

variable "docker_registry_server_url" {
  description = "The URL of the Docker registry"
  type        = string
}

variable "docker_registry_server_username" {
  description = "The username for Docker registry authentication"
  type        = string
}

variable "docker_registry_server_password" {
  description = "The password for Docker registry authentication"
  type        = string
}

variable "docker_image" {
  description = "The Docker image for the Function App"
  type        = string
}

variable "docker_image_tag" {
  description = "The Docker tag for the Function App"
  type        = string
}

variable "tags" {
  description = "A map of tags to assign to the Function App"
  type        = map(string)
  default     = {}
}
