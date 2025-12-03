import { ApiProperty } from "@nestjs/swagger";
import { IsEmail, IsString , MaxLength} from "class-validator";

export class CreateUserDto 
{
   @ApiProperty({example : 'ulisesfarias@gmail.com'})
   @IsEmail()
   email: string;

   @ApiProperty({example : 'ulises'})
   @IsString()
   @MaxLength(50)
   name: string;   
   
   @ApiProperty({example : 'farias'})
   @IsString()
   @MaxLength(20)
   lastName: string;

   @ApiProperty({example : '20442627518'})
   @IsString()
   cuil : string;
   
   @ApiProperty({example : '44262751'})
   @IsString()
   dni : string;
}