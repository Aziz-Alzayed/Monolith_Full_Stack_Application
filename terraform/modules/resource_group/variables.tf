variable "name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The Azure region where the resource group should be created"
}

variable "tags" {
  type        = map(string)
  description = "A mapping of tags to assign to the resource group"
  default     = {}
}

variable "wait_duration" {
  description = "Duration to wait after creating the resource group."
  type        = string
  default     = "20s"  # Default duration, can be overridden when calling the module
}
