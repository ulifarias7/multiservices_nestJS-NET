export class Email {
  id?: number;
  to: string;
  subject: string;
  body: string;
  templateType?: string;
  templateData?: Record<string, any>;
  sentAt?: Date;
  status: 'pending' | 'sent' | 'failed';
  errorMessage?: string;
  createdAt?: Date;

  constructor(partial: Partial<Email>) {
    Object.assign(this, partial);
  }
}