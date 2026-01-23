import { TypeOrmModuleOptions } from '@nestjs/typeorm';
import { Email } from 'src/email/domain/entities/email.entity';

export class TypeOrmConfig {
  static getOrmConfig(): TypeOrmModuleOptions {
    return {
      type: 'postgres',
      host: process.env.DB_HOST!,
      port: Number(process.env.DB_PORT),
      database: process.env.DB_DATABAS!,
      password: process.env.DB_PASS!,
      username: process.env.DB_USER!,
      entities: [Email],
      synchronize: true,
    };
  }
}