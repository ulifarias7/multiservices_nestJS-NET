import {
  IsEmail,
  IsString,
  IsOptional,
  IsArray,
  ValidateNested,
} from 'class-validator';
import { Type } from 'class-transformer';

class AttachmentDto {
  @IsString()
  filename: string;

  @IsString()
  content: string;
}

export class SendEmailDto {
  @IsEmail()
  to: string;

  @IsString()
  subject: string;

  @IsOptional()
  @IsString()
  html?: string;

  @IsOptional()
  @IsString()
  text?: string;

  @IsOptional()
  @IsArray()
  @IsEmail({}, { each: true })
  cc?: string[];

  @IsOptional()
  @IsArray()
  @IsEmail({}, { each: true })
  bcc?: string[];

  @IsOptional()
  @IsArray()
  @ValidateNested({ each: true })
  @Type(() => AttachmentDto)
  attachments?: AttachmentDto[];
}
