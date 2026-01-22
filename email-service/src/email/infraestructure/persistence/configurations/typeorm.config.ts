import { TypeOrmModuleOptions } from '@nestjs/typeorm';
import { Email } from 'src/email/domain/entities/email.entity';

export class TypeOrmConfig {
  static getOrmConfig(): TypeOrmModuleOptions {
    return {
      type: 'postgres',
      host: process.env.DB_HOST!,
      port: Number(process.env.DB_PORT),
      username: process.env.DB_EMIAL!,
      password: process.env.DB_PASS!,
      database: process.env.DB_NAME!,
      entities: [Email],
      synchronize: true,
    };
  }
}