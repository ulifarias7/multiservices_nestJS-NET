import { Email } from "../entities/email.entity";

export interface IEmailRepository {
  save(email: Email): Promise<Email>;
  findById(id: string): Promise<Email | null>;
}
