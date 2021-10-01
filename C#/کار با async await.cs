  private async void CallButton_Click(object sender, EventArgs e)
    {
        this.Text = await DoWork();
    }

    private Task DoWork()
    {
        return Task.Run(() =>
        {
            Thread.Sleep(10000);
            return "Done.";
        });