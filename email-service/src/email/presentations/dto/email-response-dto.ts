export class EmailResponseDto{
  id: number;
  to: string;
  subject: string;
  status: string;
  sentAt?: Date;
  createdAt: Date;

  constructor(partial: Partial<EmailResponseDto>) {
    Object.assign(this, partial);
  }
}