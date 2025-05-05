namespace Graduation_Project.Api.Helpers
{
    public static class EmailTemplateService
    {
        public static string GetOtpEmailBody(string userEmail, string otpCode)
        {
            return $@"
    <div style='font-family:Arial,sans-serif; max-width:600px; margin:0 auto; padding:20px; border:1px solid #ddd; border-radius:10px;'>
        <div style='text-align:center;'>
            <img src='https://marketplace.canva.com/EAE8fLYOzrA/1/0/1600w/canva-health-care-bO8TgMWVszg.jpg' alt='Pharmacy Logo' style='width:100px; margin-bottom:20px;' />
        </div>
        <h2 style='color:#2c3e50;'>Email Verification Code</h2>
        <p>Hello <strong>{userEmail.Split("@")[0]}</strong>,</p>
        <p>Your One-Time Password (OTP) to verify your email is:</p>
        <div style='font-size:24px; font-weight:bold; color:#3498db; margin:20px 0;'>{otpCode}</div>
        <p>This code is valid for the next 5 minutes. Please do not share it with anyone.</p>
        <p style='margin-top:30px;'>Thanks,<br><strong>Balto Team</strong></p>
    </div>";
        }

        public static string GetResendOtpEmailBody(string userEmail, string otpCode)
        {
            return $@"
    <div style='font-family:Arial,sans-serif; max-width:600px; margin:0 auto; padding:20px; border:1px solid #f39c12; border-radius:10px; background-color:#fcf8e3;'>
        <div style='text-align:center;'>
            <img src='https://marketplace.canva.com/EAE8fLYOzrA/1/0/1600w/canva-health-care-bO8TgMWVszg.jpg' alt='Pharmacy Logo' style='width:100px; margin-bottom:20px;' />
        </div>
        <h2 style='color:#e67e22;'>Here’s Your New OTP Code</h2>
        <p>Hello <strong>{userEmail.Split("@")[0]}</strong>,</p>
        <p>You’ve requested to resend your OTP. Here is your new verification code:</p>
        <div style='font-size:24px; font-weight:bold; color:#d35400; margin:20px 0;'>{otpCode}</div>
        <p>This code is valid for 5 minutes. If you didn’t request this, you can safely ignore this email.</p>
        <p style='margin-top:30px;'>Stay safe,<br><strong>Balto Team</strong></p>
    </div>";
        }

        public static string GetForgotPasswordOtpBody(string userEmail, string otpCode)
        {
            return $@"
    <div style='font-family:Arial,sans-serif; max-width:600px; margin:0 auto; padding:20px; border:1px solid #ddd; border-radius:10px;'>
        <div style='text-align:center;'>
            <img src='https://marketplace.canva.com/EAE8fLYOzrA/1/0/1600w/canva-health-care-bO8TgMWVszg.jpg' alt='Pharmacy Logo' style='width:100px; margin-bottom:20px;' />
        </div>
        <h2 style='color:#c0392b;'>Reset Your Password</h2>
        <p>Hello <strong>{userEmail.Split("@")[0]}</strong>,</p>
        <p>We received a request to reset your password. Please use the OTP code below to proceed:</p>
        <div style='font-size:24px; font-weight:bold; color:#e74c3c; margin:20px 0;'>{otpCode}</div>
        <p>This code is valid for the next 5 minutes. Do not share it with anyone.</p>
        <p>If you didn’t request this, you can safely ignore this email and your password will remain unchanged.</p>
        <p style='margin-top:30px;'>Best regards,<br><strong>Balto Team</strong></p>
    </div>";
        }




    }
}
