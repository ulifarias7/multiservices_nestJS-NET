import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { IEmailRepository } from 'src/email/domain/entities/repositories/email.repository.interface';
import { Email } from '../../../../domain/entities/email.entity';
import { EmailTypeOrmEntity } from '../entities/email.typeorm';

@Injectable()
export class EmailRepository implements IEmailRepository {
  constructor(
    @InjectRepository(EmailTypeOrmEntity)
    private readonly emailRepo: Repository<EmailTypeOrmEntity>,
  ) {}

  async save(email: Email): Promise<Email> {
    const emailEntity = this.emailRepo.create(email);
    const saved = await this.emailRepo.save(emailEntity);
    return new Email(saved);
  }

  async findById(id: number): Promise<Email | null> {
    const email = await this.emailRepo.findOne({ where: { id } });
    return email ? new Email(email) : null; 
  }

  async findAll(): Promise<Email[]> {
    const emails = await this.emailRepo.find({
      order: { createdAt: 'DESC' },
    });
    return emails.map((e) => new Email(e));
  }
}