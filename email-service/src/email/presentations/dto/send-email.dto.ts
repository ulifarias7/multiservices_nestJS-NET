import { IsEmail, IsString, IsOptional, IsObject } from 'class-validator';

export class SendEmailDto {
  @IsEmail()
  to: string;

  @IsString()
  subject: string;

  @IsString()
  htmlContent: string; 

  @IsString()
  @IsOptional()
  templateType?: string; 

  @IsObject()
  @IsOptional()
  templateData?: Record<string, any>;
}