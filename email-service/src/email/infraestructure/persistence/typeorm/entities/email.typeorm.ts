import { Entity, Column, PrimaryGeneratedColumn, CreateDateColumn } from 'typeorm';

@Entity('emails')
export class EmailTypeOrmEntity{
  @PrimaryGeneratedColumn()
  id:number;

  @Column()
  to:string;

  @Column()
  subject: string;

  @Column('text')
  body: string;

  @Column({ nullable: true })
  templateType?: string;

  @Column('jsonb', { nullable: true })
  templateData?: Record<string, any>;

  @Column({ nullable: true })
  sentAt?: Date;

  @Column({
    type: 'enum',
    enum: ['pending', 'sent', 'failed'],
    default: 'pending',
  })
  status: 'pending' | 'sent' | 'failed';

  @Column({ nullable: true })
  errorMessage?: string;

  @CreateDateColumn()
  createdAt: Date;
}
