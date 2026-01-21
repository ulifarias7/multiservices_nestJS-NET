import { DomainError } from "./domain.errors";

export class InvalidEmailError extends DomainError {
  readonly statusCode = 400;

  constructor(email: string) {
    super(`Invalid email format: ${email}`);
  }
}