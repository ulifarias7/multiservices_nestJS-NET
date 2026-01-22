import { IsEmail, IsString, IsOptional, IsObject } from 'class-validator';

export class SendEmailDto {
  @IsEmail()
  to: string;

  @IsString()
  subject: string;

  @IsString()
  @IsOptional()
  body?: string;

  @IsString()
  @IsOptional()
  templateType?: string; // 'welcome', 'payment', 'document-upload', 'generic'

  @IsObject()
  @IsOptional()
  templateData?: Record<string, any>;
}