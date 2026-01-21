export enum EmailStatus {
  PENDING = 'PENDING',
  SENT = 'SENT',
  FAILED = 'FAILED',
}

export class Email {
  constructor(
    public readonly id: string,
    public readonly to: string,
    public readonly subject: string,
    public readonly status: EmailStatus,
    public readonly createdAt: Date,
    public readonly sentAt?: Date,
    public readonly errorMessage?: string,
    public readonly cc?: string[],
    public readonly bcc?: string[],
    public readonly html?: string,
    public readonly text?: string,
  ) {}

  static create(data: {
    to: string;
    subject: string;
    html?: string;
    text?: string;
    cc?: string[];
    bcc?: string[];
  }): Email {
    return new Email(
      crypto.randomUUID(),
      data.to,
      data.subject,
      EmailStatus.PENDING,
      new Date(),
      undefined,
      undefined,
      data.cc,
      data.bcc,
      data.html,
      data.text,
    );
  }

  markAsSent(): Email {
    return new Email(
      this.id,
      this.to,
      this.subject,
      EmailStatus.SENT,
      this.createdAt,
      new Date(),
      undefined,
      this.cc,
      this.bcc,
      this.html,
      this.text,
    );
  }

  markAsFailed(errorMessage: string): Email {
    return new Email(
      this.id,
      this.to,
      this.subject,
      EmailStatus.FAILED,
      this.createdAt,
      undefined,
      errorMessage,
      this.cc,
      this.bcc,
      this.html,
      this.text,
    );
  }
}
