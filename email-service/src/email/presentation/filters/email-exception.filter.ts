import {
  ExceptionFilter,
  Catch,
  ArgumentsHost,
  HttpStatus,
} from '@nestjs/common';
import { Response } from 'express';
import { DomainError } from 'src/email/domain/errors/domain.errors';

@Catch(DomainError)
export class EmailExceptionFilter implements ExceptionFilter {
  catch(exception: DomainError, host: ArgumentsHost) {
    const ctx = host.switchToHttp();
    const response = ctx.getResponse<Response>();

    response.status(exception.statusCode).json({
      statusCode: exception.statusCode,
      message: exception.message,
      error: exception.name,
      timestamp: new Date().toISOString(),
    });
  }
}