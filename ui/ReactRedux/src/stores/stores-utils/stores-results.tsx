import { HttpStatusCode } from "axios";

export interface StoresResults<T = void> {
  passed: boolean;
  message?: string;
  statusCode?: HttpStatusCode;
  entity?:T
}
