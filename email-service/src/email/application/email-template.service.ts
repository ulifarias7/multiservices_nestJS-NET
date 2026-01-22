import { Injectable } from "@nestjs/common";

@Injectable()
export class EmailTempleateService{
  getTemplate(templateType: string, data: Record<string, any>): string {
    const templates = {
      welcome: this.welcomeTemplate(data),
      payment: this.paymentTemplate(data),
      'document-upload': this.documentUploadTemplate(data),
      generic: this.genericTemplate(data),
    };

    return templates[templateType] || this.genericTemplate(data);
  }
//esto no me gusta nadaaaaaaaaa deberiamos recibir el templearte desde la app madre
    private welcomeTemplate(data: Record<string, any>): string {
    return `
      <!DOCTYPE html>
      <html>
        <head>
          <style>
            body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
            .container { max-width: 600px; margin: 0 auto; padding: 20px; }
            .header { background-color: #4CAF50; color: white; padding: 20px; text-align: center; }
            .content { padding: 20px; background-color: #f9f9f9; }
            .footer { text-align: center; padding: 10px; font-size: 12px; color: #777; }
          </style>
        </head>
        <body>
          <div class="container">
            <div class="header">
              <h1>¡Bienvenido ${data.name || 'Usuario'}!</h1>
            </div>
            <div class="content">
              <p>Hola ${data.name || 'Usuario'},</p>
              <p>Gracias por registrarte en nuestra plataforma. Estamos encantados de tenerte con nosotros.</p>
              <p><strong>Email:</strong> ${data.email || ''}</p>
              ${data.activationLink ? `<p><a href="${data.activationLink}" style="background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;">Activar Cuenta</a></p>` : ''}
            </div>
            <div class="footer">
              <p>&copy; ${new Date().getFullYear()} - Todos los derechos reservados</p>
            </div>
          </div>
        </body>
      </html>
    `;
  }
   private paymentTemplate(data: Record<string, any>): string {
    return `
      <!DOCTYPE html>
      <html>
        <head>
          <style>
            body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
            .container { max-width: 600px; margin: 0 auto; padding: 20px; }
            .header { background-color: #2196F3; color: white; padding: 20px; text-align: center; }
            .content { padding: 20px; background-color: #f9f9f9; }
            .invoice { background-color: white; padding: 15px; margin: 10px 0; border-left: 4px solid #2196F3; }
          </style>
        </head>
        <body>
          <div class="container">
            <div class="header">
              <h1>Confirmación de Pago</h1>
            </div>
            <div class="content">
              <p>Hola ${data.customerName || 'Cliente'},</p>
              <p>Tu pago ha sido procesado exitosamente.</p>
              <div class="invoice">
                <p><strong>Número de transacción:</strong> ${data.transactionId || 'N/A'}</p>
                <p><strong>Monto:</strong> $${data.amount || '0.00'}</p>
                <p><strong>Fecha:</strong> ${data.date || new Date().toLocaleDateString()}</p>
                <p><strong>Método de pago:</strong> ${data.paymentMethod || 'N/A'}</p>
              </div>
              <p>Gracias por tu pago.</p>
            </div>
          </div>
        </body>
      </html>
    `;
  }
   private documentUploadTemplate(data: Record<string, any>): string {
    return `
      <!DOCTYPE html>
      <html>
        <head>
          <style>
            body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
            .container { max-width: 600px; margin: 0 auto; padding: 20px; }
            .header { background-color: #FF9800; color: white; padding: 20px; text-align: center; }
            .content { padding: 20px; background-color: #f9f9f9; }
          </style>
        </head>
        <body>
          <div class="container">
            <div class="header">
              <h1>Documento Subido</h1>
            </div>
            <div class="content">
              <p>Hola ${data.userName || 'Usuario'},</p>
              <p>Se ha subido un nuevo documento a tu cuenta.</p>
              <p><strong>Nombre del documento:</strong> ${data.documentName || 'N/A'}</p>
              <p><strong>Tipo:</strong> ${data.documentType || 'N/A'}</p>
              <p><strong>Tamaño:</strong> ${data.documentSize || 'N/A'}</p>
              <p><strong>Fecha de subida:</strong> ${data.uploadDate || new Date().toLocaleDateString()}</p>
            </div>
          </div>
        </body>
      </html>
    `;
  }
  
  private genericTemplate(data: Record<string, any>): string {
    const fieldsHtml = Object.entries(data)
      .map(([key, value]) => `<p><strong>${key}:</strong> ${value}</p>`)
      .join('');

    return `
      <!DOCTYPE html>
      <html>
        <head>
          <style>
            body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }
            .container { max-width: 600px; margin: 0 auto; padding: 20px; }
            .header { background-color: #607D8B; color: white; padding: 20px; text-align: center; }
            .content { padding: 20px; background-color: #f9f9f9; }
          </style>
        </head>
        <body>
          <div class="container">
            <div class="header">
              <h1>${data.title || 'Notificación'}</h1>
            </div>
            <div class="content">
              ${data.message ? `<p>${data.message}</p>` : ''}
              ${fieldsHtml}
            </div>
          </div>
        </body>
      </html>
    `;
  }
}
