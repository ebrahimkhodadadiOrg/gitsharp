private void textBox1_TextChanged(object sender, EventArgs e)
{
    if (textBox1.Text == "" || textBox1.Text == "0") return;                         
    decimal price;                                                                   
    price = decimal.Parse(textBox1.Text, System.Globalization.NumberStyles.Currency);
    textBox1.Text = price.ToString("#,#");                                           
    textBox1.SelectionStart = textBox1.Text.Length;                                  
}