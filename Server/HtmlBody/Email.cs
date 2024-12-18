namespace Server.HtmlBody
{
    public class Email
    {
        public static string ConfirmEmail(int code)
        {
            string html = @"
        <!DOCTYPE html>
        <html lang=""pl"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    color: #333;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    padding: 20px;
                    border-radius: 8px;
                }
                .header {
                    text-align: center;
                    padding-bottom: 20px;
                }
                .header h1 {
                    margin: 0;
                    color: #333;
                }
                .content {
                    text-align: center;
                    padding: 20px 0;
                }
                .code {
                    font-size: 24px;
                    font-weight: bold;
                    color: #333;
                    background-color: #f1f1f1;
                    padding: 10px;
                    border-radius: 4px;
                    display: inline-block;
                }
                .footer {
                    text-align: center;
                    padding-top: 20px;
                    font-size: 12px;
                    color: #777;
                }
            </style>
        </head>
        <body>
            <div class=""container"">
                <div class=""header"">
                    <h1>Potwierdź swój email</h1>
                </div>
                <div class=""content"">
                    <p>Twój kod weryfikacyjny to:</p>
                    <div class=""code"">"
+
code
+
@"</div>
                </div>
                <div class=""footer"">
                    <p>Jeśli nie prosiłeś o ten email, zignoruj go.</p>
                </div>
            </div>
        </body>
        </html>
        ";
            return html;
        }


    }
}
