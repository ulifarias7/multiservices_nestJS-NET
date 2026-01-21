import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Email } from '../../domain/entities/email.entity';
import { IEmailRepository } from '../../domain/repositories/email.repository.interface';
import { EmailOrmEntity } from './email.orm-entity';

@Injectable()
export class EmailRepository implements IEmailRepository {
  constructor(
    @InjectRepository(EmailOrmEntity)
    private readonly repository: Repository<EmailOrmEntity>,
  ) {}

  async save(email: Email): Promise<Email> {
    const ormEntity = this.toOrmEntity(email);
    const saved = await this.repository.save(ormEntity);
    return this.toDomain(saved);
  }

  async findById(id: string): Promise<Email | null> {
    const ormEntity = await this.repository.findOne({ where: { id } });
    return ormEntity ? this.toDomain(ormEntity) : null;
  }

  private toOrmEntity(email: Email): EmailOrmEntity {
    const entity = new EmailOrmEntity();
    entity.id = email.id;
    entity.to = email.to;
    entity.subject = email.subject;
    entity.status = email.status;
    entity.createdAt = email.createdAt;
    entity.sentAt = email.sentAt;
    entity.errorMessage = email.errorMessage;
    entity.cc = email.cc;
    entity.bcc = email.bcc;
    entity.html = email.html;
    entity.text = email.text;
    return entity;
  }

  private toDomain(entity: EmailOrmEntity): Email {
    return new Email(
      entity.id,
      entity.to,
      entity.subject,
      entity.status,
      entity.createdAt,
      entity.sentAt,
      entity.errorMessage,
      entity.cc,
      entity.bcc,
      entity.html,
      entity.text,
    );
  }
}
