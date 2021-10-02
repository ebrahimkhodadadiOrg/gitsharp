

BULK INSERT Sales
FROM 'C:\temp\1500000 Sales Records.csv'
WITH (FIRSTROW = 2, --if you have title in first row either just change it to 1--
    FIELDTERMINATOR = ',',
    ROWTERMINATOR='\n',
    BATCHSIZE=250000,
    MAXERRORS=2);	

