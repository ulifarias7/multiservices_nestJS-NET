import { Entity, Column, PrimaryColumn, CreateDateColumn } from 'typeorm';
import { EmailStatus } from '../../domain/entities/email.entity';

@Entity('emails')
export class EmailOrmEntity {
  @PrimaryColumn('uuid')
  id: string;

  @Column()
  to: string;

  @Column()
  subject: string;

  @Column({ type: 'enum', enum: EmailStatus })
  status: EmailStatus;

  @CreateDateColumn({ name: 'created_at' })
  createdAt: Date;

  @Column({ name: 'sent_at', nullable: true })
  sentAt?: Date;

  @Column({ name: 'error_message', nullable: true, type: 'text' })
  errorMessage?: string;

  @Column({ type: 'simple-array', nullable: true })
  cc?: string[];

  @Column({ type: 'simple-array', nullable: true })
  bcc?: string[];

  @Column({ type: 'text', nullable: true })
  html?: string;

  @Column({ type: 'text', nullable: true })
  text?: string;
}