using auth0rize.auth.domain.User;
using System.Net;
using System.Net.Mail;

namespace auth0rize.auth.notification
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        #region Inyeccion
        private readonly string _emailAddress = "";
        private string _password = "";
        private string _host = "";
        private int _port = 0;

        public UserNotificationRepository(string emailAddress, string password, string host, int port)
        {
            _emailAddress = emailAddress;
            _password = password;
            _host = host;
            _port = port;
        }
        #endregion

        public async Task Registration(string url, string to)
        {
            string html = @$"
                            <!DOCTYPE html>
                            <html lang=""es"">
                            <head>
                                <meta charset=""UTF-8"">
                                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                <title>Confirmar Correo Electrónico</title>
                                <style>
                                    * {{
                                        margin: 0;
                                        padding: 0;
                                        box-sizing: border-box;
                                        background: linear-gradient(135deg, #0f172a 0%, #14b8a6 100%);
                                    }}

                                    body {{
                                        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
                                        min-height: 100vh;
                                        display: flex;
                                        align-items: center;
                                        justify-content: center;
                                        padding: 20px;
                                    }}

                                    .container {{
                                        background: white;
                                        border-radius: 16px;
                                        box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
                                        padding: 48px 40px;
                                        max-width: 480px;
                                        width: 100%;
                                        text-align: center;
                                        position: relative;
                                        overflow: hidden;
                                    }}

                                    .container::before {{
                                        content: '';
                                        position: absolute;
                                        top: 0;
                                        left: 0;
                                        right: 0;
                                        height: 4px;
                                        background: linear-gradient(90deg, #0f172a, #14b8a6);
                                    }}

                                    .icon {{
                                        width: 80px;
                                        height: 80px;
                                        background: linear-gradient(135deg, #0f172a, #14b8a6);
                                        border-radius: 50%;
                                        display: flex;
                                        align-items: center;
                                        justify-content: center;
                                        margin: 0 auto 24px;
                                        position: relative;
                                    }}

                                    .icon::after {{
                                        content: '✉';
                                        font-size: 36px;
                                        color: white;
                                    }}

                                    .title {{
                                        font-size: 28px;
                                        font-weight: 700;
                                        color: #1a1a1a;
                                        margin-bottom: 12px;
                                        line-height: 1.2;
                                    }}

                                    .subtitle {{
                                        font-size: 16px;
                                        color: #666;
                                        margin-bottom: 32px;
                                        line-height: 1.5;
                                    }}

                                    .email-display {{
                                        background: #f8f9fa;
                                        border: 2px solid #e9ecef;
                                        border-radius: 8px;
                                        padding: 16px;
                                        margin-bottom: 32px;
                                        font-family: 'Courier New', monospace;
                                        font-size: 14px;
                                        color: #495057;
                                        word-break: break-all;
                                    }}

                                    .confirm-button {{
                                        background: linear-gradient(135deg, #0f172a, #14b8a6);
                                        color: white;
                                        border: none;
                                        border-radius: 12px;
                                        padding: 16px 32px;
                                        font-size: 16px;
                                        font-weight: 600;
                                        cursor: pointer;
                                        transition: all 0.3s ease;
                                        text-decoration: none;
                                        display: inline-block;
                                        margin-bottom: 24px;
                                        min-width: 200px;
                                        position: relative;
                                        overflow: hidden;
                                    }}

                                    .confirm-button:hover {{
                                        transform: translateY(-2px);
                                        box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
                                    }}

                                    .confirm-button:active {{
                                        transform: translateY(0);
                                    }}

                                    .confirm-button::before {{
                                        content: '';
                                        position: absolute;
                                        top: 0;
                                        left: -100%;
                                        width: 100%;
                                        height: 100%;
                                        background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
                                        transition: left 0.5s;
                                    }}

                                    .confirm-button:hover::before {{
                                        left: 100%;
                                    }}

                                    .alternative-link {{
                                        display: block;
                                        color: #667eea;
                                        text-decoration: none;
                                        font-size: 14px;
                                        font-weight: 500;
                                        margin-bottom: 32px;
                                        transition: color 0.3s ease;
                                    }}

                                    .alternative-link:hover {{
                                        color: #764ba2;
                                        text-decoration: underline;
                                    }}

                                    .help-text {{
                                        font-size: 14px;
                                        color: #888;
                                        line-height: 1.5;
                                        margin-bottom: 16px;
                                    }}

                                    .support-link {{
                                        color: #667eea;
                                        text-decoration: none;
                                        font-weight: 500;
                                    }}

                                    .support-link:hover {{
                                        text-decoration: underline;
                                    }}

                                    .divider {{
                                        height: 1px;
                                        background: #e9ecef;
                                        margin: 24px 0;
                                    }}

                                    .security-note {{
                                        background: #fff3cd;
                                        border: 1px solid #ffeaa7;
                                        border-radius: 8px;
                                        padding: 12px;
                                        font-size: 13px;
                                        color: #856404;
                                        margin-top: 24px;
                                    }}

                                    @media (max-width: 480px) {{
                                        .container {{
                                            padding: 32px 24px;
                                            margin: 10px;
                                        }}

                                        .title {{
                                            font-size: 24px;
                                        }}

                                        .confirm-button {{
                                            width: 100%;
                                            min-width: auto;
                                        }}
                                    }}

                                    @media (prefers-reduced-motion: reduce) {{
                                        .confirm-button {{
                                            transition: none;
                                        }}
            
                                        .confirm-button:hover {{
                                            transform: none;
                                        }}
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class=""container"">
                                    <div class=""icon"" role=""img"" aria-label=""Icono de correo electrónico""></div>
        
                                    <h1 class=""title"">Confirma tu correo electrónico</h1>
                                    <p class=""subtitle"">
                                        Bienvenido a [Auth0rize], hemos enviado un enlace de confirmación a tu dirección de correo electrónico.
                                    </p>

                                    <div class=""email-display"" role=""region"" aria-label=""Dirección de correo"">
                                        {to}
                                    </div>

                                    <a href=""{url}"" class=""confirm-button"" role=""button"" aria-describedby=""confirm-help"">
                                        Confirmar correo electrónico
                                    </a>

                                    <a href=""{url}"" class=""alternative-link"">
                                        Acceder con enlace alternativo
                                    </a>

                                    <div class=""divider"" role=""separator""></div>

                                    <p class=""help-text"" id=""confirm-help"">
                                        ¿No recibiste el correo? Revisa tu carpeta de spam.</a>
                                    </p>

                                    <div class=""security-note"">
                                        <strong>Nota de seguridad:</strong> Este enlace expirará en 1 hora por tu seguridad.
                                    </div>
                                </div>
                            </body>
                            </html>";
            MailAddress fromMail = new MailAddress(_emailAddress, "[Auth0rize]");
            MailAddress toMail = new MailAddress(to);
            var smtp = new SmtpClient
            {
                Host = _host,
                Port = _port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, _password)
            };
            using (var message = new MailMessage(fromMail, toMail)
            {
                Subject = "Confirm user register.",
                Body = html,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        public Task RegistrationConfirm(string url, string to)
        {
            throw new NotImplementedException();
        }

        public Task RegistrationError(string url, string to)
        {
            throw new NotImplementedException();
        }

        public Task LoginCorrect(string to)
        {
            throw new NotImplementedException();
        }
    }
}
