import { Email } from "../email.entity";

export interface IEmailRepository{
  save(email: Email): Promise<Email>;
  findById(id: number): Promise<Email | null>;
  findAll(): Promise<Email[]>;
}