public BackgroundWorker bk = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            bk.DoWork += bk_DoWork;
            bk.ProgressChanged += backgroundWorker1_ProgressChanged;
            bk.WorkerReportsProgress = true;
        }

private void StartCounting_Click(object sender, RoutedEventArgs e)
        {
            bk.RunWorkerAsync();
        }


private void bk_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                bk.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pb.Value = e.ProgressPercentage;
        }

--------------------------------------------------------------------------------------------------
private bool closePending;
bkFastAgi.WorkerSupportsCancellation = true;

protected override void OnFormClosing(FormClosingEventArgs e) {
    if (backgroundWorker1.IsBusy) {
        closePending = true;
        backgroundWorker1.CancelAsync();
        e.Cancel = true;
        this.Enabled = false;   // or this.Hide()
        return;
    }
    base.OnFormClosing(e);
}

void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
    if (closePending) this.Close();
    closePending = false;
    // etc...
}